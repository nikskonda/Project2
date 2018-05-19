using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cells_db.model
{
    public class Constans
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public string Unit { get; set; }

        public Constans()
        {
            this.Id = 0;
            this.Name = "Unknown";
            this.Value = 0;
            this.Unit = "";
        }

        public Constans(int id, string name, double value, string unit)
        {
            this.Id = id;
            this.Name = name;
            this.Value = value;
            this.Unit = unit;
        }
    }
}
