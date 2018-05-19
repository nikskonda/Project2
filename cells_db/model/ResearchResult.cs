using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cells_db.model
{
   public class ResearchResult
    {
        public int id {get; set;}
        public int id_detail { get; set; }
        public string name { get; set; }
        public string parameter { get; set; }
        public int iteration { get; set; }
        public double stress { get; set; }
        public double displacement { get; set; }

        public ResearchResult()
        {
            name = "Research dated " + DateTime.Now.ToString();
        }

        public ResearchResult(int id_detail, string parameter, int iteration, double stress, double displacement)
        {
            this.id_detail = id_detail;
            name = "Research dated " + DateTime.Now.ToString();
            this.parameter = parameter;
            this.iteration = iteration;
            this.stress = stress;
            this.displacement = displacement;
        }

        public ResearchResult(int id_detail, string name, string parameter, int iteration, double stress, double displacement)
        {
            this.id_detail = id_detail;
            this.name = name;
            this.parameter = parameter;
            this.iteration = iteration;
            this.stress = stress;
            this.displacement = displacement;
        }

        public ResearchResult(int id, int id_detail, string name, string parameter, int iteration, double stress, double displacement)
        {
            this.id = id;
            this.id_detail = id_detail;
            this.name = name;
            this.parameter = parameter;
            this.iteration = iteration;
            this.stress = stress;
            this.displacement = displacement;
        }

        public override string ToString()
        {
            return this.name + ":" + Environment.NewLine +
                this.parameter + Environment.NewLine +
                "Iterations: " + this.iteration.ToString() + Environment.NewLine +
                "Stress: " + this.stress.ToString() + Environment.NewLine +
                "Displacement: " + this.displacement.ToString();
        }
    }
}
