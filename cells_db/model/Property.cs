using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cells_db.model
{
    public class Property
    {
        public int Id { get; set; }

        public int IdParent { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public Value Value { get; set; }

        public Property()
        {
            this.Id = 0;
            this.Name = "Unknown";
            this.Unit = "";
            this.Value = new Value();
        }

        public Property(int id, string name, string unit)
        {
            this.Id = id;
            this.Name = name;
            this.Unit = unit;
            this.Value = null;
        }

        public Property(string name, string unit)
        {
            this.Id = 0;
            this.Name = name;
            this.Unit = unit;
            this.Value = null;
        }

        public Property(string name)
        {
            this.Id = 0;
            this.Name = name;
            this.Unit = "";
            this.Value = null;
        }

        public Property(int id, string name, string unit, Value value)
        {
            this.Id = id;
            this.Name = name;
            this.Unit = unit;
            this.Value = value;
        }

        public Property(int id, int idParent, string name, string unit, Value value)
        {
            Id = id;
            IdParent = idParent;
            Name = name;
            Unit = unit;
            Value = value;
        }

        public string GetStringNameAndUnit()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.Name).Append(" / ").Append(this.Unit);

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
           
            if (obj == null)
            {
                return false;
            }
            else
            {
                Property pr = obj as Property;
                bool f = true;
                if (!this.Name.Equals(pr.Name))
                {
                    f = false;
                }
                if (!this.Unit.Equals(pr.Unit))
                {
                    f = false;
                }
                if (this.Value != null && pr.Value != null)
                if (!this.Value.Equals(pr.Value))
                {
                    f = false;
                }

                return f;
            }

        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Value != null)
            {
                sb.Append(this.Name).Append(": ").Append(this.Value.value).Append(" ").Append(this.Unit);
            }
            

            return sb.ToString();
        }
    }
}
