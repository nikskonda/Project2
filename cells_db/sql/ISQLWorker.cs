
using cells_db.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cells_db.sql
{
    public interface ISQLWorker
    {

        List<Detail> GetDetails();

        List<Cell> GetCells();

        List<Material> GetMaterials();

        List<CellType> GetCellTypes();

        List<ResearchResult> GetResearchResults();

        List<ResearchResult> GetResearchResults(int id_detail);

        List<Property> GetProperties(ParentType parentType);

        bool AddMaterial(Material material);

        bool AddCell(Cell cell);

        bool AddDetail(Detail detail);

        bool AddCellType(CellType cellType);

        bool AddProperty(Property p, ParentType parentType);

        bool AddResearchResult(ResearchResult researchResult);

        bool delDetail(Detail detail);
    }
}
