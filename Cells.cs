using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
/*
           ________________
          /|              /|
         / |             / |
        /  |            /  |
       /   |           /   |
      /    |          /    |
     /     |_ _ _ _ _/_ _ _|
   H|---------------|      /
   E|     /         |     /T
   I|    /          |    /H
   G|   /           |   /G
   H|  /            |  /N
   T| /             | /E
    |/______________|/L
           WIDTH

*/

namespace Cube
{
    class Cells
    {
        private int iterationNumber;
        private double VHole;
        private double k; //расстояние между рёбрами тела и сторонами отверстий
        private int accuracy = 5;

        //Body
        private double VBody;
        private double bodyWidth;
        private double bodyHeight;    
        private double bodyLenght;

        //Hole
        private double holeWidth;
        private double holeHeight;
        private double holeLenght;

        public List<Complex> debugList;

        public Cells(double bodyWidth, double bodyHeight, double bodyLenght)
        {
            this.bodyWidth = bodyWidth;
            this.bodyHeight = bodyHeight;
            this.bodyLenght = bodyLenght;
            this.VBody = bodyWidth * bodyHeight * bodyLenght;
        }

        public void KCalculation() {
            double A, B, C;
            if (iterCheck())
            {
                double t = Math.Sqrt(Math.Pow(2, iterationNumber));
                A = ((bodyWidth+bodyHeight)/(t+1) + bodyLenght)*(-1);
                B = bodyWidth*bodyHeight + bodyLenght * (t + 1) * (bodyWidth + bodyHeight);
                B /= Math.Pow(t+1,2);
                C = (bodyWidth * bodyHeight * bodyLenght) - ((VHole * Math.Pow(t,2))/Math.Pow(2, iterationNumber));
                C /= Math.Pow(t + 1, 2) *(-1);
            }
            else
            {
                double p = Math.Sqrt(Math.Pow(2, iterationNumber-1));
                A = ((bodyWidth/(2*p+1)) + (bodyHeight/(p+1)) + bodyLenght)*(-1);
                B = bodyWidth * bodyHeight + bodyHeight * bodyLenght*(2*p+1) + bodyWidth* bodyLenght*(p+1);
                B /= (2 * p + 1) * (p + 1);
                C = bodyWidth * bodyHeight * bodyLenght - (VHole * 2 * Math.Pow(p, 2) / Math.Pow(2, iterationNumber));
                C /= (2 * p + 1) * (p + 1)*(-1);
            }
            //////////////////////

            debugList = GetRoots(A, B, C);

            k = -1.0;

            foreach (Complex element in debugList)
            {
                if (element.Real < 0) {
                    continue;
                }
                if (element.Real == -1.0) {
                    k = -1.0;
                    break;
                }

                k = element.Real;
                double buff1, buff2, buff3;

                if (iterCheck())
                {
                    double t = Math.Sqrt(Math.Pow(2, iterationNumber));
                    SetHoleWidthOddIteration(); SetHoleHeightOddIteration(); SetHoleLenght();
                    if (holeWidth < 0 || holeHeight < 0 || holeLenght < 0) {
                        continue;
                    }
                    buff1 = t * holeWidth + (t + 1) * k;
                    buff2 = t * holeHeight + (t + 1) * k;
                    buff3 = k + holeLenght;
                }
                else {
                    double p = Math.Sqrt(Math.Pow(2, iterationNumber - 1));
                    SetHoleWidthNotOddIteration(); SetHoleHeightNotOddIteration(); SetHoleLenght();
                    if (holeWidth < 0 || holeHeight < 0 || holeLenght < 0)
                    {
                        continue;
                    }
                    buff1 = 2 * p * holeWidth + (2 * p + 1) * k;
                    buff2 = p * holeHeight + (p + 1) * k;
                    buff3 = k + holeLenght;
                }

                //MessageBox.Show(Math.Round(buff1, accuracy).CompareTo(Math.Round(bodyWidth, accuracy)).ToString());
                //MessageBox.Show(buff1.ToString() + " " + bodyWidth.ToString());

                //Comporation(buff1, bodyWidth);
                /*
                MessageBox.Show(Math.Round(buff2, accuracy).CompareTo(Math.Round(bodyHeight, accuracy)).ToString());
                MessageBox.Show(buff2.ToString() + " " + bodyHeight.ToString());

                MessageBox.Show(Math.Round(buff3, accuracy).CompareTo(Math.Round(bodyLenght, accuracy)).ToString());
                MessageBox.Show(buff3.ToString() + " " + bodyLenght.ToString());
                */
                //MessageBox.Show(buff1.CompareTo(bodyWidth).ToString());

                if (Comporation(buff1, bodyWidth)
                    && Comporation(buff2, bodyHeight)
                    && Comporation(buff3, bodyLenght)
                    )
                {
                    MessageBox.Show(true.ToString());
                    return;
                }
                else { k = -1.0; }

            }

        }

        public bool iterCheck()
        {
            if (iterationNumber % 2 == 0) { return true; }
            else { return false; }
        }

        private /*void*/ List<Complex> GetRoots(double a, double b, double c)
        {
            var q = (Math.Pow(a, 2) - 3 * b) / 9;
            var r = (2 * Math.Pow(a, 3) - 9 * a * b + 27 * c) / 54;

            if (Math.Pow(r, 2) < Math.Pow(q, 3))
            {
                var t = Math.Acos(r / Math.Sqrt(Math.Pow(q, 3))) / 3;
                var x1 = -2 * Math.Sqrt(q) * Math.Cos(t) - a / 3;
                var x2 = -2 * Math.Sqrt(q) * Math.Cos(t + (2 * Math.PI / 3)) - a / 3;
                var x3 = -2 * Math.Sqrt(q) * Math.Cos(t - (2 * Math.PI / 3)) - a / 3;
                return new List<Complex> { x1, x2, x3 };
            }
            else
            {
                
                //resultTextBox.Text += "Colmplex Way" + Environment.NewLine;
                
                var A = -Math.Sign(r) * Math.Pow(Math.Abs(r) + Math.Sqrt(Math.Pow(r, 2) - Math.Pow(q, 3)), (1.0 / 3.0));
                var B = (A == 0) ? 0.0 : q / A;
                
                var x1 = (A + B) - a / 3;
                return new List<Complex> { x1 };
                /*
                var x2 = -(A + B) / 2 - (a / 3) + (Complex.ImaginaryOne * Math.Sqrt(3) * (A - B) / 2);
                var x3 = -(A + B) / 2 - (a / 3) - (Complex.ImaginaryOne * Math.Sqrt(3) * (A - B) / 2);

                if (A == B)
                {
                    x2 = -A - a / 3;
                    return new List<Complex> { x1, x2 };
                }
                return new List<Complex> { x1, x2, x3 };
                */
            }
        }

        private bool Comporation(double buff, double bodySize) {
            //MessageBox.Show(Math.Round(buff, accuracy).ToString() + " " + Math.Round(bodySize, accuracy).ToString());
            if (buff < 0) {
                return false;
            }

            for(int i = accuracy; i > 1; i--)
            {
                //MessageBox.Show(Math.Round(buff, i).CompareTo(Math.Round(bodySize, i)).ToString());
                //MessageBox.Show(Math.Round(buff, i).ToString() + " " + Math.Round(bodySize, i).ToString());
                if (Math.Round(buff, i).CompareTo(Math.Round(bodySize, i)) == 0) {
                    return true;
                }
            }
            return false;
        }

        public int GetIterationNumber() { return this.iterationNumber; }
        public void SetIterationNumber(int iterationNumber) { this.iterationNumber = iterationNumber; }

        public double GetBodyWidth() { return bodyWidth; }
        public double GetBodyHeight() { return bodyHeight; }
        public double GetBodyLenght() { return bodyLenght; }

        public double GetK() { return this.k; }

        public void SetAccuracy(int accuracy) { this.accuracy = accuracy; }
        public int GetAccuracy() { return this.accuracy; }

        //Hole
        private void SetHoleWidthOddIteration() {
            double t = Math.Sqrt(Math.Pow(2, iterationNumber));
            this.holeWidth = bodyWidth - (t+1)*k;
            this.holeWidth /= t;
            this.holeWidth = Math.Round(this.holeWidth, accuracy);
        }
        private void SetHoleWidthNotOddIteration()
        {
            double p = Math.Sqrt(Math.Pow(2, iterationNumber-1));
            this.holeWidth = bodyWidth - (2*p + 1) * k;
            this.holeWidth /= 2*p;
            this.holeWidth = Math.Round(this.holeWidth, accuracy);
        }
        public double GetHoleWidth() { return this.holeWidth; }


        private void SetHoleHeightOddIteration() {
            double t = Math.Sqrt(Math.Pow(2, iterationNumber));
            this.holeHeight = bodyHeight - (t + 1) * k;
            this.holeHeight /= t;
            this.holeHeight = Math.Round(this.holeHeight, accuracy);
        }
        private void SetHoleHeightNotOddIteration() {
            double p = Math.Sqrt(Math.Pow(2, iterationNumber-1));
            this.holeHeight = bodyHeight - (p + 1) * k;
            this.holeHeight /= p;
            this.holeHeight = Math.Round(this.holeHeight, accuracy);
        }
        public double GetHoleHeight() { return this.holeHeight; }


        private void SetHoleLenght() {
            this.holeLenght = bodyLenght - k;
            this.holeLenght = Math.Round(this.holeLenght, accuracy);
        }
        public double GetHoleLenght() { return this.holeLenght; }
     
        public double GetBodyV(){ return this.VBody; }

        public void SetHoleV(double V) { this.VHole = V; }
        public void SetHoleVWithMultiplier(double multiplier) { this.VHole = VBody * multiplier ; }
        public void SetHoleVWithMultiplier(double numerator, double denominator)
        { this.VHole = VBody * numerator / denominator; }
        public double GetVHole() { return this.VHole; }

        public bool isAvailable()
        {
            return k > 0 ? true : false;
        }

    }
}
