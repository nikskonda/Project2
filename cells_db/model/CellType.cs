using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cells_db.model
{
    public class CellType
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public CellType()
        {
            Id = 0;
            TypeName = "Unknown";
        }

        public CellType(int id, string typeName)
        {
            Id = id;
            TypeName = typeName;
        }

        public override string ToString()
        {
            return "Type: " + TypeName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                CellType ct = obj as CellType;
                bool f = true;
                if (!this.TypeName.Equals(ct.TypeName))
                {
                    f = false;
                }
                return f;
            }

        }
    }
}
