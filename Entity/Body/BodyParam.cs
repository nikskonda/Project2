using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    class BodyParam
    {
        private double V;
        private double Width;
        private double Height;
        private double Lenght;

        Array faces; // Список граней тела

        public BodyParam(double bodyWidth, double bodyHeight, double bodyLenght)
        {
            this.Width = bodyWidth;
            this.Height = bodyHeight;
            this.Lenght = bodyLenght;
            this.V = bodyWidth * bodyHeight * bodyLenght;
        }

        public double GetWidth() { return Width; }
        public double GetHeight() { return Height; }
        public double GetLenght() { return Lenght; }
        public double GetV() { return this.V; }

        public void SetFaces(Array array) { faces = array; }
        public Array GetFaces() { return faces; }
    }
}
