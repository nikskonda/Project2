using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

namespace Cube
{
    class Cells
    {
        BodyParam body;

        private int iterationNumber; // число, в которое возводится число 2 для определения кол-ва отверстий
        private double k; // расстояние между рёбрами тела и сторонами отверстий
        private int accuracy = 5; // количество знаков после запятой при рисовании

        //Hole
        private double VCells; // должен быть
        private double VCellsFactical; // Фактический объём
        private double cellWidth;
        private double cellHeight;
        private double cellLenght;
        private int[] cellsNum;

        private List<Complex> rootsList;

        public Cells(BodyParam body)
        {
            this.body = body;
        }

        public void KCalculation()
        {
            double A, B, C;
            //Вычисляем элементы кубического уравнения
            //x^3 + Ax^2 + Bx + C = 0
            if (iterCheck())
            {
                double t = Math.Sqrt(Math.Pow(2, iterationNumber));
                A = ((body.GetWidth() + body.GetHeight()) / (t + 1) + body.GetLenght()) * (-1);
                B = body.GetWidth() * body.GetHeight() + body.GetLenght() * (t + 1) * (body.GetWidth() + body.GetHeight());
                B /= Math.Pow(t + 1, 2);
                C = (body.GetWidth() * body.GetHeight() * body.GetLenght()) - ((VCells * Math.Pow(t, 2)) / Math.Pow(2, iterationNumber));
                C /= Math.Pow(t + 1, 2) * (-1);
            }
            else
            {
                double p = Math.Sqrt(Math.Pow(2, iterationNumber - 1));
                A = ((body.GetWidth() / (2 * p + 1)) + (body.GetHeight() / (p + 1)) + body.GetLenght()) * (-1);
                B = body.GetWidth() * body.GetHeight() + body.GetHeight() * body.GetLenght() * (2 * p + 1) + body.GetWidth() * body.GetLenght() * (p + 1);
                B /= (2 * p + 1) * (p + 1);
                C = body.GetWidth() * body.GetHeight() * body.GetLenght() - (VCells * 2 * Math.Pow(p, 2) / Math.Pow(2, iterationNumber));
                C /= (2 * p + 1) * (p + 1) * (-1);
            }
            //////////////////////

            //Получаем корни уравнения
            rootsList = GetRoots(A, B, C);
            k = -1.0;

            foreach (Complex element in rootsList)
            {
                if (element.Real < 0)
                {
                    continue;
                }
                if (element.Real == -1.0)
                {
                    k = -1.0;
                    break;
                }

                k = element.Real;
                double buff1, buff2, buff3;

                //Вычисляем предположительные значения сторон отверстия
                if (iterCheck())
                {
                    double t = Math.Sqrt(Math.Pow(2, iterationNumber));
                    SetCellsWidthOddIteration(); SetCellsHeightOddIteration(); SetCellsLenght();
                    if (cellWidth < 0 || cellHeight < 0 || cellLenght < 0)
                    {
                        continue;
                    }
                    buff1 = t * cellWidth + (t + 1) * k;
                    buff2 = t * cellHeight + (t + 1) * k;
                    buff3 = k + cellLenght;
                }
                else
                {
                    double p = Math.Sqrt(Math.Pow(2, iterationNumber - 1));
                    SetCellsWidthNotOddIteration(); SetCellsHeightNotOddIteration(); SetCellsLenght();
                    if (cellWidth < 0 || cellHeight < 0 || cellLenght < 0)
                    {
                        continue;
                    }
                    buff1 = 2 * p * cellWidth + (2 * p + 1) * k;
                    buff2 = p * cellHeight + (p + 1) * k;
                    buff3 = k + cellLenght;
                }

                if (Comporation(buff1, body.GetWidth()) && Comporation(buff2, body.GetHeight())
                    && Comporation(buff3, body.GetLenght()))
                {
                    SetCellsNum();
                    SetVCellsFactical();
                    return;
                }
                else { k = -1.0; }

            }

        }

        /// <summary>
        /// Проверяет итерацию на чётность/нечётность
        /// </summary>
        /// <returns>Возвращает true если итерация чётная</returns>
        public bool iterCheck() { return iterationNumber % 2 == 0 ? true : false; }

        /// <summary>
        /// Находит корни кубического уравнения
        /// </summary>
        /// <param name="a">Значение при x^2</param>
        /// <param name="b">Значение при x</param>
        /// <param name="c">Свободный член</param>
        /// <returns>Спиок найденных корней</returns>
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
                var A = -Math.Sign(r) * Math.Pow(Math.Abs(r) + Math.Sqrt(Math.Pow(r, 2) - Math.Pow(q, 3)), (1.0 / 3.0));
                var B = (A == 0) ? 0.0 : q / A;

                var x1 = (A + B) - a / 3;
                return new List<Complex> { x1 };
                //Часть кода, вычисляющая комплексные значения
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

        /// <summary>
        /// Сравнение 2 чисел при помощи изменения точности
        /// </summary>
        /// <param name="param1">Число 1</param>
        /// <param name="param2">Число 2</param>
        /// <returns>Возвращает true, если числа идентичны при определённой точности</returns>
        private bool Comporation(double param1, double param2)
        {
            for (int i = accuracy; i > 1; i--)
            {
                if (Math.Round(param1, i).CompareTo(Math.Round(param2, i)) == 0) { return true; }
            }
            return false;
        }

        public int GetIterationNumber() { return this.iterationNumber; }
        public void SetIterationNumber(int iterationNumber) { this.iterationNumber = iterationNumber; }

        public double GetK() { return Math.Round(this.k, accuracy); }

        public void SetAccuracy(int accuracy) { this.accuracy = accuracy; }
        public int GetAccuracy() { return this.accuracy; }

        //Cells
        public void SetCellsNum()
        {
            this.cellsNum = new int[2];

            if (iterCheck())
            {
                this.cellsNum[0] = (int)Math.Sqrt(Math.Pow(2, iterationNumber)); //row
                this.cellsNum[1] = this.cellsNum[0];
            }
            else
            {
                this.cellsNum[0] = (int)Math.Sqrt(Math.Pow(2, iterationNumber - 1)); //column
                this.cellsNum[1] = 2 * this.cellsNum[0];
            }
        }

        public int CellsInRowNumber() { return this.cellsNum[0]; }
        public int CellsInColumnNumber() { return this.cellsNum[1]; }

        private void SetCellsWidthOddIteration()
        {
            double t = Math.Sqrt(Math.Pow(2, iterationNumber));
            this.cellWidth = body.GetWidth() - (t + 1) * k;
            this.cellWidth /= t;
            this.cellWidth = Math.Round(this.cellWidth, accuracy);
        }
        private void SetCellsWidthNotOddIteration()
        {
            double p = Math.Sqrt(Math.Pow(2, iterationNumber - 1));
            this.cellWidth = body.GetWidth() - (2 * p + 1) * k;
            this.cellWidth /= 2 * p;
            this.cellWidth = Math.Round(this.cellWidth, accuracy);
        }
        public double GetHoleWidth() { return this.cellWidth; }


        private void SetCellsHeightOddIteration()
        {
            double t = Math.Sqrt(Math.Pow(2, iterationNumber));
            this.cellHeight = body.GetHeight() - (t + 1) * k;
            this.cellHeight /= t;
            this.cellHeight = Math.Round(this.cellHeight, accuracy);
        }
        private void SetCellsHeightNotOddIteration()
        {
            double p = Math.Sqrt(Math.Pow(2, iterationNumber - 1));
            this.cellHeight = body.GetHeight() - (p + 1) * k;
            this.cellHeight /= p;
            this.cellHeight = Math.Round(this.cellHeight, accuracy);
        }
        public double GetHoleHeight() { return this.cellHeight; }

        private void SetCellsLenght()
        {
            this.cellLenght = body.GetLenght() - k;
            this.cellLenght = Math.Round(this.cellLenght, accuracy);
        }
        public double GetHoleLenght() { return this.cellLenght; }

        public void SetCellsV(double V) { this.VCells = V; }
        public void SetCellsVWithMultiplier(double multiplier) { this.VCells = body.GetV() * multiplier; }
        public void SetCellsVWithMultiplier(double numerator, double denominator)
        { this.VCells = body.GetV() * numerator / denominator; }
        public double GetCellsV() { return this.VCells; }

        public double GetVCellsFactical() { return Math.Round(this.VCellsFactical, accuracy); }
        private void SetVCellsFactical() { this.VCellsFactical = cellWidth * cellHeight * cellLenght * CellsInRowNumber() * CellsInColumnNumber(); }

        public bool isAvailable() { return k > 0 ? true : false; }
    }
}
