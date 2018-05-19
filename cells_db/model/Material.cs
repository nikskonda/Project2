using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cells_db.model
{
    public class Material
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Property> Properties { get; set; }

        public Material()
        {
            this.Id = 0;
            this.Name = "Unknown";
            this.Description = "Unknown";
            this.Properties = new List<Property>();
        }

        public Material(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Properties = null;
        }

        public Material(int id, string name, string description, List<Property> property)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Properties = property;
        }

        public override bool Equals(object obj)
        {

            if (obj == null)
            {
                return false;
            }
            else
            {
                Material mat = obj as Material;
                bool f = true;
                if (!this.Name.Equals(mat.Name))
                {
                    f = false;
                }
                if (!this.Description.Equals(mat.Description))
                {
                    f = false;
                }
                //if (!this.Properties.Equals(mat.Properties))
                //{
                //    f = false;
                //}
                return f;
            }

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name: ").Append(this.Name).Append(Environment.NewLine).Append("Desc: ").Append(this.Description).Append(Environment.NewLine).Append("Properties:").Append(Environment.NewLine);
            foreach (Property pr in this.Properties)
            {
                sb.Append(pr).Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
