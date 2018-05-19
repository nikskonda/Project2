using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cells_db.model
{
    public class CellStructure
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public List<Property> Properties { get; set; }

        public List<Cell> Cells { get; set; }

        public CellStructure()
        {
            Id = 0;
            Description = "Unknown";
            Cells = new List<Cell>();
            Properties = new List<Property>();
        }

        public CellStructure(int id, string description, List<Property> properties, List<Cell> cells)
        {
            Id = id;
            Description = description;
            Properties = properties;
            Cells = cells;
        }

        public string GetStringProperties()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Parameters:").Append(Environment.NewLine);
            foreach (Property p in this.Properties)
            {
                sb.Append(p).Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public string GetStringCells()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Cells:").Append(Environment.NewLine);
            foreach (Cell c in this.Cells)
            {
                sb.Append(c.Name).Append(" ").Append(c.Description).Append(Environment.NewLine);
            }
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
                CellStructure cs = obj as CellStructure;
                bool f = true;

                if (!this.Cells.Equals(cs.Cells))
                {
                    f = false;
                }

                if (!this.Description.Equals(cs.Description))
                {
                    f = false;
                }

                if (!this.Properties.Equals(cs.Properties))
                {
                    f = false;
                }
                return f;
            }

        }
    }
}
