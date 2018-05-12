using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Models
{
    public class Value
    {
        public int Id { get; set; }

        public double value { get; set; }

        public Value()
        {
            this.Id = 0;
            this.value = 0;
        }

        public Value(double value)
        {
            this.Id = 0;
            this.value = value;
        }

        public Value(int id, double value)
        {
            this.Id = id;
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                Value v = obj as Value;
                bool f = true;
                if (!this.value.Equals(v.value))
                {
                    f = false;
                }               
                return f;
            }

        }
    }
}
