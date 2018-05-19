using cells_db.exception;
using cells_db.sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cells_db.model
{
    public class Data
    {
        public List<Detail> details { get; set; }
        public List<Material> materials { get; set; }
        public List<Cell> cells { get; set; }
        public List<CellType> cellTypes { get; set; }

        public List<Property> detProperties { get; set; }
        public List<Property> matProperties { get; set; }
        public List<Property> cellProperties { get; set; }
        public List<Property> csProperties { get; set; }

        private ISQLWorker sqlWorker;

        public Data()
        {
            details = new List<Detail>();
            materials = new List<Material>();
            cells = new List<Cell>();
            cellTypes = new List<CellType>();

            detProperties = new List<Property>();
            matProperties = new List<Property>();
            cellProperties = new List<Property>();
            csProperties = new List<Property>();
        }

        public Data(ISQLWorker sqlWorker)
        {
            details = new List<Detail>();
            materials = new List<Material>();
            cells = new List<Cell>();
            cellTypes = new List<CellType>();

            detProperties = new List<Property>();
            matProperties = new List<Property>();
            cellProperties = new List<Property>();
            csProperties = new List<Property>();

            this.sqlWorker = sqlWorker;
        }

        public void loadDetails(ISQLWorker sqlWorker)
        {
            details.Clear();
            details = sqlWorker.GetDetails();
        }

        public void loadMaterials(ISQLWorker sqlWorker)
        {
            materials.Clear();
            materials = sqlWorker.GetMaterials();
        }

        public void loadCells(ISQLWorker sqlWorker)
        {
            cells.Clear();
            cells = sqlWorker.GetCells();
        }
        public void loadCellTypes(ISQLWorker sqlWorker)
        {
            cellTypes.Clear();
            cellTypes = sqlWorker.GetCellTypes();
        }

        public void loadDetProperties(ISQLWorker sqlWorker)
        {
            detProperties.Clear();
            detProperties = sqlWorker.GetProperties(ParentType.Detail);
        }

        public void loadMatProperties(ISQLWorker sqlWorker)
        {
            matProperties.Clear();
            matProperties = sqlWorker.GetProperties(ParentType.Material);
        }

        public void loadCellProperties(ISQLWorker sqlWorker)
        {
            cellProperties.Clear();
            cellProperties = sqlWorker.GetProperties(ParentType.Cell);
        }

        public void loadCSProperties(ISQLWorker sqlWorker)
        {
            csProperties.Clear();
            csProperties = sqlWorker.GetProperties(ParentType.CelluralStructure);
        }

        public void loadAll(ISQLWorker sqlWorker)
        {
            this.loadDetails(sqlWorker);
            this.loadMaterials(sqlWorker);
            this.loadCells(sqlWorker);
            this.loadCellTypes(sqlWorker);

            this.loadCellProperties(sqlWorker);
            this.loadCSProperties(sqlWorker);
            this.loadMatProperties(sqlWorker);
            this.loadDetProperties(sqlWorker);
        }

        public void loadDetails()
        {
            if (sqlWorker == null) throw new SQLWorkerException("SQLWorker is equal to null.");
            details.Clear();
            details = sqlWorker.GetDetails();
        }

        public void loadMaterials()
        {
            if (sqlWorker == null) throw new SQLWorkerException("SQLWorker is equal to null.");

            materials.Clear();
            materials = sqlWorker.GetMaterials();
        }

        public void loadCells()
        {
            if (sqlWorker == null) throw new SQLWorkerException("SQLWorker is equal to null.");

            cells.Clear();
            cells = sqlWorker.GetCells();
        }
        public void loadCellTypes()
        {
            if (sqlWorker == null) throw new SQLWorkerException("SQLWorker is equal to null.");

            cellTypes.Clear();
            cellTypes = sqlWorker.GetCellTypes();
        }

        public void loadDetProperties()
        {
            if (sqlWorker == null) throw new SQLWorkerException("SQLWorker is equal to null.");

            detProperties.Clear();
            detProperties = sqlWorker.GetProperties(ParentType.Detail);
        }

        public void loadMatProperties()
        {
            if (sqlWorker == null) throw new SQLWorkerException("SQLWorker is equal to null.");

            matProperties.Clear();
            matProperties = sqlWorker.GetProperties(ParentType.Material);
        }

        public void loadCellProperties()
        {
            if (sqlWorker == null) throw new SQLWorkerException("SQLWorker is equal to null.");

            cellProperties.Clear();
            cellProperties = sqlWorker.GetProperties(ParentType.Cell);
        }

        public void loadCSProperties()
        {
            if (sqlWorker == null) throw new SQLWorkerException("SQLWorker is equal to null.");

            csProperties.Clear();
            csProperties = sqlWorker.GetProperties(ParentType.CelluralStructure);
        }

        public void loadAll()
        {
            if (sqlWorker == null) throw new SQLWorkerException("SQLWorker is equal to null.");

            this.loadDetails(sqlWorker);
            this.loadMaterials(sqlWorker);
            this.loadCells(sqlWorker);
            this.loadCellTypes(sqlWorker);

            this.loadCellProperties(sqlWorker);
            this.loadCSProperties(sqlWorker);
            this.loadMatProperties(sqlWorker);
            this.loadDetProperties(sqlWorker);
        }

        public bool AddNewResearchResultAndUpdateDetails(ResearchResult researchResult, ISQLWorker sqlWorker)
        {
            if (sqlWorker == null) return false;
            if (sqlWorker.AddResearchResult(researchResult))
            {
                this.loadDetails();
                return true;
            }
            else return false;
        }

        public bool AddNewResearchResultAndUpdateDetails(ResearchResult researchResult)
        {
            if (sqlWorker == null) return false;
            if (sqlWorker.AddResearchResult(researchResult))
            {
                this.loadDetails();
                return true;
            }
            else return false;
        }

    }
}
