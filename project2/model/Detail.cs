using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Models
{
    public class Detail
    {
        public int Id { get; set; }

        public Material Material { get; set; }

        public CellStructure CellStructure { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Property> Properties { get; set; }

        public Detail()
        {
            Id = 0;
            Material = new Material();
            CellStructure = new CellStructure();
            Name = "Unknown";
            Description = "Unknown";
            Properties = new List<Property>();
        }        

        public Detail(int id, Material material, CellStructure cS, string name, string description, List<Property> property)
        {
            Id = id;
            Material = material;
            CellStructure = cS;
            Name = name;
            Description = description;
            Properties = property;
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

        public string GetStringNameAndDesc()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name: ").Append(this.Name).Append(Environment.NewLine).Append("Desc: ").Append(this.Description).Append(Environment.NewLine);
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
                Detail det = obj as Detail;
                bool f = true;
                if (!this.Name.Equals(det.Name))
                {
                    f = false;
                }
                if (!this.Description.Equals(det.Description))
                {
                    f = false;
                }
                //if (!this.Properties.Equals(det.Properties))
                //{
                //    f = false;
                //}
                if (!this.Material.Equals(det.Material))
                {
                    f = false;
                }
                if (!this.CellStructure.Equals(det.CellStructure))
                {
                    f = false;
                }
                return f;
            }

        }

    }
}
