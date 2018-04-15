﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cube.Models;

namespace Cube
{
    public enum ParentType
    {
        Detail, Material, Cell, CelluralStructure
    }

    class SQLWorker : ISQLWorker
    {
        private const String PCname = "NIKITA";
        private const String DBname = "Project2";



        public SqlConnection GetSqlConnection(string namePC, string nameDB)
        {           
            StringBuilder sb = new StringBuilder("server = ");
            sb.Append(namePC).Append("; database = ").Append(nameDB).Append("; Integrated Security = true");
            SqlConnection sql = new SqlConnection(sb.ToString());
            return sql;
        }

        public List<Detail> GetDetails()
        {
            List<Detail> details = new List<Detail>();
            try
            {
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("SELECT * from Details", sqlConnection);
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

                        details.Add(detail);
                    }
                }
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
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("SELECT Id_Cell from CellStructures_Cells WHERE (Id_CellStructure=@IdCS)", sqlConnection);
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
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("SELECT Properties.Id_Property, Properties.[Name], Properties.Unit from Properties " +
                    "left join ParentTypes on Properties.PropertyType = ParentTypes.Id_Type " +
                    "WHERE (ParentTypes.[Name] = @ParentType)", sqlConnection);
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
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("SELECT Properties.Id_Property, [Values].Id_Parent, Id_Value, Properties.[Name], [Value],  Properties.Unit from [Values] " +
                    "left join ParentTypes on[Values].Id_ParentType = ParentTypes.Id_Type " +
                    "left join Properties on[Values].Id_Property = Properties.Id_Property " +
                    "WHERE (ParentTypes.[Name] = @ParentType)", sqlConnection);
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
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("SELECT * from Cellular_Structures", sqlConnection);
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
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("SELECT Cells.Id_Cell, Cells.[Name], Cells.[Description], Cells.Id_Cell_Type, Cell_Types.[Name] from Cells " +
                    "left join Cell_Types on (Cells.Id_Cell_Type=Cell_Types.Id_Cell_Type)", sqlConnection);
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
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("SELECT * from Materials", sqlConnection);
                sqlConnection.Open();

                SqlDataReader sDR = sql.ExecuteReader();

                if (sDR.HasRows)
                {
                    while (sDR.Read())
                    {
                        Material material = new Material();
                        material.Id = sDR.GetInt32(0);
                        material.Name = sDR.GetString(1);
                        material.Description = sDR.GetString(1);
                        material.Properties = GetPropertyValues(ParentType.Material).FindAll(x => x.IdParent == material.Id);

                        list.Add(material);
                    }
                }
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return list;
        }

        public List<CellType> GetCellTytes()
        {
            List<CellType> list = new List<CellType>();
            try
            {
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("SELECT * from Cell_Types", sqlConnection);
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
                    SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                    SqlCommand sql = new SqlCommand("INSERT INTO Cell_Types (Name) VALUES (@name);", sqlConnection);
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
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("SELECT ParentTypes.Id_Type from ParentTypes where(ParentTypes.[Name] = @ParentType)", sqlConnection);
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
            }
            catch (System.Exception ex)
            {
                //DialogManager.showDialogError("Возможна ошибка базы данных! Проверьте!", "Внимание!");
                Console.WriteLine(ex);
            }
            return id;
        }

        public bool AddMaterial(Material m)
        {
            bool f = false;
            try
            {
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("INSERT INTO Materials (Name, Description) VALUES (@name, @desc);", sqlConnection);
                sqlConnection.Open();
                sql.Parameters.AddWithValue("@name", m.Name);
                sql.Parameters.AddWithValue("@desc", m.Description);
                sql.ExecuteReader();
                sqlConnection.Close();

                List<Material> list = GetMaterials();
                Material newM = list.FindLast(x => x.Name.Equals(m.Name));
                if (newM != null)
                {
                    f = AddValues(m.Properties, newM.Id, GetIdParentType(ParentType.Material));
                }          
                
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
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("INSERT INTO Cells (Id_Cell_Type, Name, Description) VALUES (@idType, @name, @desc);", sqlConnection);
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
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand sql = new SqlCommand("INSERT INTO [Values] (Id_Property, Id_Parent, [Value], Id_ParentType) VALUES (@idProp, @idparent, @value, @idParType);", sqlConnection);
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
                    SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                    SqlCommand sql = new SqlCommand("INSERT INTO CellStructures_Cells (Id_Cell, Id_CellStructure) VALUES (@cell, @cs);", sqlConnection);
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
                SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                SqlCommand cmd = new SqlCommand("InsertCS", sqlConnection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@descCS", System.Data.SqlDbType.VarChar, 400));
                cmd.Parameters["@descCS"].Value = cs.Description;

                cmd.Parameters.Add(new SqlParameter("@idCS", System.Data.SqlDbType.Int, 8));
                cmd.Parameters["@idCS"].Direction = System.Data.ParameterDirection.Output;

                sqlConnection.Open();
                cmd.ExecuteNonQuery();

                cs.Id = (int)cmd.Parameters["@idCS"].Value;

                //SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                //SqlCommand sql = new SqlCommand("INSERT INTO Cellular_Structures (Description) VALUES (@desc);", sqlConnection);
                //sqlConnection.Open();
                //sql.Parameters.AddWithValue("@desc", cs.Description);
                //sql.ExecuteReader();
                //sqlConnection.Close();

                //CellStructure newCS = GetCellStructures().FindLast(x => x.Description.Equals(cs.Description));
                //if (newCS != null)
                {
                    f = AddValues(cs.Properties, cs.Id, GetIdParentType(ParentType.CelluralStructure));
                    //cs.Id = newCS.Id;
                    f = AddListCells(cs);
                }

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
                        SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
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

                        sqlConnection.Open();
                        cmd.ExecuteNonQuery();

                        detail.Id = (int)cmd.Parameters["@idDetail"].Value;


                        detail.CellStructure.Id = newCS.Id;

                        //SqlConnection sqlConnection = GetSqlConnection(SQLWorker.PCname, SQLWorker.DBname);
                        //SqlCommand sql = new SqlCommand("INSERT INTO Details(Name, Description, Id_CellStructure, Id_Material) VALUES (@name, @desc, @idCS, @idMat);", sqlConnection);
                        //sqlConnection.Open();
                        //sql.Parameters.AddWithValue("@name", detail.Name);
                        //sql.Parameters.AddWithValue("@desc", detail.Description);
                        //sql.Parameters.AddWithValue("@idCS", detail.CellStructure.Id);
                        //sql.Parameters.AddWithValue("@idMat", detail.Material.Id);
                        //sql.ExecuteReader();
                        //sqlConnection.Close();

                        //Detail newD = GetDetails().FindLast(x => (x.Name.Equals(detail.Name) && x.Description.Equals(detail.Description)));
                        //if (newD != null)
                        {
                            f = AddValues(detail.Properties, detail.Id, GetIdParentType(ParentType.Detail));
                        }

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

        

    }
}
