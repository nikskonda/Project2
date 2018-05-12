using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Models
{
    public class Data
    {
        public List<Detail> details { get; set; }

        public List<Property> detProperties { get; set; }
        public List<Property> matProperties { get; set; }
        public List<Property> cellProperties { get; set; }
        public List<Property> csProperties { get; set; }
        public List<Property> objProperties { get; set; }
        public List<Material> materials { get; set; }

        public Data()
        {
            details = new List<Detail>();
            detProperties = new List<Property>();
            matProperties = new List<Property>();
            cellProperties = new List<Property>();
            csProperties = new List<Property>();
            objProperties = new List<Property>();
            materials = new List<Material>();
        }
    }
}
