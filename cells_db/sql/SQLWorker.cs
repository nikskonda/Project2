using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using cells_db.util;
using cells_db.model;
using cells_db.model;

namespace cells_db.sql
{
    public enum ParentType
    {
        Detail, Material, Cell, CelluralStructure
    }

    class SQLWorker : ISQLWorker
    {
        private String PCname;
        private String DBname;

        private static DBCreater dbCreater;

        private SqlConnection sqlConnection;

        public static bool IsCreate(String PCname, String DBname)
        {          
            try
            {
                StringBuilder sb = new StringBuilder("server = ");
                sb.Append(PCname).Append("; Integrated Security = true");
                SqlConnection sqlConnection = new SqlConnection(sb.ToString());               
                SqlCommand sql = new SqlCommand("if db_id(@name) is not null select 'true' else select 'false'", sqlConnection);
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@name", DBname);
                SqlDataReader sDR = sql.ExecuteReader();
                String result = "false";
                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        result = sDR.GetString(0);                        
                    }
                }
                sqlConnection.Close();
                if (result.Equals("true"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public SQLWorker(String PCname, String DBname)
        {
            this.PCname = PCname;
            this.DBname = DBname;
            if (IsCreate(this.PCname, this.DBname))
            {
                sqlConnection = GetSqlConnection();
            }
            else
            {
                DialogManager.showDialogError("Connection error");
            }
        }

        private SqlConnection GetSqlConnection()
        {           
            StringBuilder sb = new StringBuilder("server = ");
            sb.Append(this.PCname).Append("; database = ").Append(this.DBname).Append("; Integrated Security = true");
            SqlConnection sql = new SqlConnection(sb.ToString());
            return sql;
        }

        public List<Detail> GetDetails()
        {
            List<Detail> details = new List<Detail>();
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("SELECT * from Details", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();             
                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        Detail detail = new Detail();
                        detail.Id = sDR.GetInt32(0);

                        detail.Material = GetMaterials().FindLast(x => x.Id == sDR.GetInt32(1));
                        detail.CellStructure = GetCellStructures().FindLast(x => x.Id == sDR.GetInt32(2));

                        detail.Name = sDR.GetString(3);
                        detail.Description = sDR.GetString(4);   
                        
                        detail.Properties = GetPropertyValues(ParentType.Detail).FindAll(x => x.IdParent == detail.Id);
                        detail.ResearchResults = GetResearchResults(detail.Id);

                        details.Add(detail);
                    }
                }
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return details;
        }

        private List<int> GetListCellInCS(int IdCS)
        {
            List<int> list = new List<int>();
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("SELECT Id_Cell from CellStructures_Cells WHERE (Id_CellStructure=@IdCS)", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@IdCS", IdCS);

                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        list.Add(sDR.GetInt32(0));
                    }
                }
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return list;
        }

        public List<Property> GetProperties(ParentType parentType)
        {
            List<Property> list = new List<Property>();

            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("SELECT Properties.Id_Property, Properties.[Name], Properties.Unit from Properties " +
                    "left join ParentTypes on Properties.PropertyType = ParentTypes.Id_Type " +
                    "WHERE (ParentTypes.[Name] = @ParentType)", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@ParentType", parentType.ToString());

                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        Property p = new Property();
                        p.Id = sDR.GetInt32(0);                      
                        p.Name = sDR.GetString(1);
                        p.Unit = sDR.GetString(2);
                        list.Add(p);
                    }
                }
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return list;
        }

        public List<Property> GetPropertyValues(ParentType parentType)
        {
            List<Property> list = new List<Property>();

            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("SELECT Properties.Id_Property, [Values].Id_Parent, Id_Value, Properties.[Name], [Value],  Properties.Unit from [Values] " +
                    "left join ParentTypes on[Values].Id_ParentType = ParentTypes.Id_Type " +
                    "left join Properties on[Values].Id_Property = Properties.Id_Property " +
                    "WHERE (ParentTypes.[Name] = @ParentType)", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@ParentType", parentType.ToString());

                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        Property p = new Property();
                        p.Id = sDR.GetInt32(0);
                        p.IdParent = sDR.GetInt32(1);
                        p.Value.Id = sDR.GetInt32(2);
                        p.Name = sDR.GetString(3);
                        p.Value.value = sDR.GetDouble(4);
                        p.Unit = sDR.GetString(5);
                        list.Add(p);
                    }
                }
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return list;
        }

        public List<CellStructure> GetCellStructures()
        {
            List<CellStructure> list = new List<CellStructure>();
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("SELECT * from Cellular_Structures", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();

                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        CellStructure cs = new CellStructure();

                        cs.Id = sDR.GetInt32(0);
                        cs.Description = sDR.GetString(1);


                        List<int> idCells = GetListCellInCS(cs.Id);
                        List<Cell> cells = GetCells();

                        foreach (int id in idCells)
                        {
                            cs.Cells.Add(cells.FindLast(x => x.Id == id));
                        }

                        cs.Properties = GetPropertyValues(ParentType.CelluralStructure).FindAll(x => x.IdParent == cs.Id);

                        list.Add(cs);
                    }
                }
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return list;
        }

        public List<Cell> GetCells()
        {
            List<Cell> list = new List<Cell>();

            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("SELECT Cells.Id_Cell, Cells.[Name], Cells.[Description], Cells.Id_Cell_Type, Cell_Types.[Name] from Cells " +
                    "left join Cell_Types on (Cells.Id_Cell_Type=Cell_Types.Id_Cell_Type)", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();

                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        Cell cell = new Cell();

                        cell.Id = sDR.GetInt32(0);
                        cell.Name = sDR.GetString(1);
                        cell.Description = sDR.GetString(2);

                        cell.CellType.Id = sDR.GetInt32(3);
                        cell.CellType.TypeName = sDR.GetString(4);

                        cell.Properties = GetPropertyValues(ParentType.Cell).FindAll(x => x.IdParent == cell.Id);

                        list.Add(cell);
                    }
                }
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return list;
        }

        public List<Material> GetMaterials()
        {
            List<Material> list = new List<Material>();
            try
            {
                sqlConnection = GetSqlConnection();
                sqlConnection.Close();
                SqlCommand sql = new SqlCommand("SELECT * from Materials", sqlConnection);
                
                sqlConnection.Open();

                //SqlDataReader sDR = sql.ExecuteReader(CommandBehavior.CloseConnection);
                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        Material material = new Material();
                        material.Id = sDR.GetInt32(0);
                        material.Name = sDR.GetString(1);
                        material.Description = sDR.GetString(2);
                        material.Properties = GetPropertyValues(ParentType.Material).FindAll(x => x.IdParent == material.Id);

                        list.Add(material);
                    }
                }
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return list;
        }

        public List<CellType> GetCellTypes()
        {
            List<CellType> list = new List<CellType>();
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("SELECT * from Cell_Types", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();      
                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        CellType ct = new CellType();
                        ct.Id = sDR.GetInt32(0);
                        ct.TypeName = sDR.GetString(1);
                        list.Add(ct);
                    }
                }
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return list;
        }

        public bool AddCellType(CellType ct)
        {
                bool f = false;
                try
                {
                    sqlConnection = GetSqlConnection();
                    SqlCommand sql = new SqlCommand("INSERT INTO Cell_Types (Name) VALUES (@name);", sqlConnection);
                    sqlConnection.Close();
                    sqlConnection.Open();
                    sql.Parameters.AddWithValue("@name", ct.TypeName);
                    sql.ExecuteReader();
                    sqlConnection.Close();
                    f = true;
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return f;

        }

        private int GetIdParentType(ParentType parentType)
        {
            int id = 0;
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("SELECT ParentTypes.Id_Type from ParentTypes where(ParentTypes.[Name] = @ParentType)", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@ParentType", parentType.ToString());
                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {                      
                        id = sDR.GetInt32(0);
                    }
                }
                sqlConnection.Close();
            }

            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return id;
        }

        public bool AddMaterial(Material material)
        {
            bool f = false;
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("INSERT INTO Materials (Name, Description) VALUES (@name, @desc);", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@name", material.Name);
                sql.Parameters.AddWithValue("@desc", material.Description);
                sql.ExecuteReader();
                sqlConnection.Close();

                List<Material> list = GetMaterials();
                Material newM = list.FindLast(x => x.Name.Equals(material.Name));
                if (newM != null)
                {
                    f = AddValues(material.Properties, newM.Id, GetIdParentType(ParentType.Material));
                }
                sqlConnection.Close();
                f = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return f;
        }

        public bool AddCell(Cell cell)
        {
            bool f = false;
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("INSERT INTO Cells (Id_Cell_Type, Name, Description) VALUES (@idType, @name, @desc);", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@name", cell.Name);
                sql.Parameters.AddWithValue("@desc", cell.Description);
                sql.Parameters.AddWithValue("@idType", cell.CellType.Id);
                sql.ExecuteReader();
                sqlConnection.Close();

                List <Cell> list = GetCells();
                Cell newC = list.FindLast(x => x.Name.Equals(cell.Name));
                if (newC != null)
                {
                    f = AddValues(cell.Properties, newC.Id, GetIdParentType(ParentType.Cell));
                }
                sqlConnection.Close();
                f = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return f;

        }

        public bool AddValues(List<Property> list, int parentId, int parentTypeId)
        {
            bool f = true;
            foreach (Property p in list)
            {
                if (p.Value == null)
                {
                    f = false;
                }
                p.IdParent = parentId;
                f = AddValue(p, parentTypeId);
            }
            return f;
        }

        public bool AddValue(Property p, int parentTypeId)
        {
            bool f = false;
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("INSERT INTO [Values] (Id_Property, Id_Parent, [Value], Id_ParentType) VALUES (@idProp, @idparent, @value, @idParType);", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@idProp", p.Id);
                sql.Parameters.AddWithValue("@idparent", p.IdParent);
                sql.Parameters.AddWithValue("@value", p.Value.value);
                sql.Parameters.AddWithValue("@idParType", parentTypeId);
                sql.ExecuteReader();
                sqlConnection.Close();
                f = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return f;

        }

        public bool AddListCells(CellStructure cellStr)
        {
            bool f = false;
            try
            {
                foreach (Cell c in cellStr.Cells)
                {
                    sqlConnection = GetSqlConnection();
                    SqlCommand sql = new SqlCommand("INSERT INTO CellStructures_Cells (Id_Cell, Id_CellStructure) VALUES (@cell, @cs);", sqlConnection);
                    sqlConnection.Close();
                    sqlConnection.Open();
                    sql.Parameters.AddWithValue("@cs", cellStr.Id);
                    sql.Parameters.AddWithValue("@cell", c.Id);
                    sql.ExecuteReader();
                    sqlConnection.Close();                 
                }
                f = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return f;
        }

        public bool AddCellStructure(CellStructure cs)
        {
            bool f = false;
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand cmd = new SqlCommand("InsertCS", sqlConnection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@descCS", System.Data.SqlDbType.VarChar, 400));
                cmd.Parameters["@descCS"].Value = cs.Description;

                cmd.Parameters.Add(new SqlParameter("@idCS", System.Data.SqlDbType.Int, 8));
                cmd.Parameters["@idCS"].Direction = System.Data.ParameterDirection.Output;

                sqlConnection.Close();
                sqlConnection.Open();
                cmd.ExecuteNonQuery();

                cs.Id = (int)cmd.Parameters["@idCS"].Value;

                f = AddValues(cs.Properties, cs.Id, GetIdParentType(ParentType.CelluralStructure));
                f = AddListCells(cs);

                sqlConnection.Close();
                f = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return f;
        }

        public bool AddDetail(Detail detail)
        {
            bool f = false;
            try
            {
                if (AddCellStructure(detail.CellStructure))
                {
                    CellStructure newCS = GetCellStructures().FindLast(x => x.Description.Equals(detail.CellStructure.Description));
                    if (newCS != null)
                    {
                        sqlConnection = GetSqlConnection();
                        SqlCommand cmd = new SqlCommand("InsertDetail", sqlConnection);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@idMat", System.Data.SqlDbType.Int, 8));
                        cmd.Parameters["@idMat"].Value = detail.Material.Id;

                        cmd.Parameters.Add(new SqlParameter("@idCS", System.Data.SqlDbType.Int, 8));
                        cmd.Parameters["@idCS"].Value = detail.CellStructure.Id;

                        cmd.Parameters.Add(new SqlParameter("@nameDetail", System.Data.SqlDbType.VarChar, 50));
                        cmd.Parameters["@nameDetail"].Value = detail.Name;

                        cmd.Parameters.Add(new SqlParameter("@descDet", System.Data.SqlDbType.VarChar, 400));
                        cmd.Parameters["@descDet"].Value = detail.Name;

                        cmd.Parameters.Add(new SqlParameter("@idDetail", System.Data.SqlDbType.Int, 8));
                        cmd.Parameters["@idDetail"].Direction = System.Data.ParameterDirection.Output;

                        sqlConnection.Close();
                        sqlConnection.Open();
                        cmd.ExecuteNonQuery();

                        detail.Id = (int)cmd.Parameters["@idDetail"].Value;


                        detail.CellStructure.Id = newCS.Id;
                    
                        {
                            f = AddValues(detail.Properties, detail.Id, GetIdParentType(ParentType.Detail));
                        }
                        sqlConnection.Close();
                        f = true;
                    }            
                }

                
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return f;
        }

        public bool AddProperty(Property p, ParentType parentType)
        {
            bool f = false;
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("INSERT INTO Properties ([Name], Unit, PropertyType) VALUES (@name, @unit, @parentType);", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@name", p.Name);
                sql.Parameters.AddWithValue("@unit", p.Unit);
                sql.Parameters.AddWithValue("@parentType", GetIdParentType(parentType));
                sql.ExecuteReader();
                sqlConnection.Close();
                f = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return f;

        }



        public static bool createDataBase(String PCname, String DBname, String path)
        {
            dbCreater = new DBCreater(DBname, path);
            bool f = false;
            StringBuilder sb = new StringBuilder("server = ");
            sb.Append(PCname).Append("; Integrated Security = true");
            try
            {
                SqlConnection sqlConnection = new SqlConnection(sb.ToString());
                
                    SqlCommand sql = new SqlCommand(dbCreater.getCreateDBSting(), sqlConnection);
                    sqlConnection.Open();
                    sql.ExecuteReader();
                    sqlConnection.Close();
                f = true;
                SQLWorker w = new SQLWorker(PCname, DBname);
                f = addNewTables(w) && addStoreProc(w);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            dbCreater = null;
            return f;
        }

        private static bool addStoreProc(SQLWorker worker)
        {
            bool f = false;
            try { 
                List<String> list = dbCreater.getListProc();
                SqlConnection con = worker.GetSqlConnection();
                foreach (String str in list)
                {
                    SqlCommand sql = new SqlCommand(str, con);
                    //sql.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    sql.ExecuteReader();
                    con.Close();
                }
                f = true;

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return f;
        }

        private static bool addNewTables(SQLWorker worker)
        {
            bool f = false;
            try { 
                List<String> list = dbCreater.getList();
                SqlConnection con = worker.GetSqlConnection();
                foreach (String str in list)
                {
                    SqlCommand sql = new SqlCommand(str, con);
                    con.Open();
                    sql.ExecuteReader();
                    con.Close();
                }
                f = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return f;
        }

        public static bool delDB(String PCname, String DBname)
        {
            bool f = false;
            StringBuilder sb = new StringBuilder("server = ");
            sb.Append(PCname).Append("; Integrated Security = true");
            try
            {
                SqlConnection sqlConnection = new SqlConnection(sb.ToString());

                {
                    SqlCommand sql = new SqlCommand("USE master DROP DATABASE "+DBname, sqlConnection);
                    sqlConnection.Open();
                    sql.ExecuteReader();
                    sqlConnection.Close();
                }
                f = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return f;
        }

        public List<ResearchResult> GetResearchResults(int id_detail)
        {
            List<ResearchResult> list = new List<ResearchResult>();
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("SELECT * from ResearchResult where Id_Detail="+id_detail, sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();
                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        ResearchResult result = new ResearchResult();

                        result.id = sDR.GetInt32(0);
                        result.id_detail = sDR.GetInt32(1);
                        result.name = sDR.GetString(2);
                        result.parameter = sDR.GetString(3);
                        result.iteration = sDR.GetInt32(4);
                        result.stress = sDR.GetDouble(5);
                        result.displacement = sDR.GetDouble(6);

                        list.Add(result);
                    }
                }
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return list;
        }

        public List<ResearchResult> GetResearchResults()
        {
            List<ResearchResult> list = new List<ResearchResult>();
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("SELECT * from ResearchResult", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();

                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        ResearchResult result = new ResearchResult();

                        result.id = sDR.GetInt32(0);
                        result.id_detail = sDR.GetInt32(1);
                        result.name = sDR.GetString(2);
                        result.parameter = sDR.GetString(3);
                        result.iteration = sDR.GetInt32(4);
                        result.stress = sDR.GetFloat(5);
                        result.displacement = sDR.GetFloat(6);

                        list.Add(result);
                    }
                }
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return list;
        }

        public bool AddResearchResult(ResearchResult researchResult)
        {
            bool f = false;
            try
            {
                sqlConnection = GetSqlConnection();
                SqlCommand sql = new SqlCommand("INSERT INTO ResearchResult ([Id_Detail], [Name], [Parameter], [Iteration], [Stress], [Displacement]) VALUES (@idDet, @name, @param, @iter, @stress, @displ);", sqlConnection);
                sqlConnection.Close();
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@idDet", researchResult.id_detail);
                sql.Parameters.AddWithValue("@name", researchResult.name);
                sql.Parameters.AddWithValue("@param", researchResult.parameter);
                sql.Parameters.AddWithValue("@iter", researchResult.iteration);
                sql.Parameters.AddWithValue("@stress", researchResult.stress);
                sql.Parameters.AddWithValue("@displ", researchResult.displacement);
                sql.ExecuteReader();
                sqlConnection.Close();
                f = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            return f;
        }
    }
}
