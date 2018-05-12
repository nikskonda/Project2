using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Models
{
    public class Cell
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CellType CellType { get; set; }

        public List<Property> Properties { get; set; }
    

        public Cell()
        {
            this.Id = 0;
            this.Name = "Unknown";
            this.Description = "Unknown";
            this.CellType = new CellType();
            this.Properties = new List<Property>();
        }

        public Cell(int id, string name, string desc, CellType cellType, List<Property> property)
        {
            Id = id;
            Name = name;
            Description = desc;
            CellType = cellType;
            Properties = property;
        }

        public string GetNameAndDesc()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name: ").Append(this.Name).Append(" Description: ").Append(this.Description).Append(" ").Append(this.CellType);      
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
                Cell cell = obj as Cell;


                bool f = true;

                if (!this.Name.Equals(cell.Name))
                {
                    f = false;
                }
                if (!this.Description.Equals(cell.Description))
                {
                    f = false;
                }
                if (!this.CellType.Equals(cell.CellType))
                {
                    f = false;
                }
                //if (!this.Properties.Equals(cell.Properties))
                //{
                //    f = false;
                //}
                return f;
            }

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name: ").Append(this.Name).Append(Environment.NewLine).Append("Description: ").Append(this.Description).Append(Environment.NewLine).Append(this.CellType)
                .Append(Environment.NewLine).Append("Parameters:").Append(Environment.NewLine);
            foreach (Property p in this.Properties)
            {
                sb.Append(p).Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
