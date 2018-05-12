using Cube.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube
{
    public interface ISQLWorker
    {

        List<Detail> GetDetails();

        List<Cell> GetCells();

        List<Material> GetMaterials();

    }
}
