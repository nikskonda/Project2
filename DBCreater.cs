using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube
{
    class DBCreater
    {
        private String dbCreate;
        private String cellType;
        private String cells;
        private String cellularStructure;
        private String cc;
        private String materials;
        private String details;
        private String constant;
        private String parentType;
        private String properties;
        private String result;
        private String values;
        private String prt;
        private String insertCS;
        private String insertDetail;

        public DBCreater(String name)
        {
            dbCreate = "USE [master]" +
            "  CREATE DATABASE[" + name + "]" +
             " CONTAINMENT = NONE" +
             " ON PRIMARY" +
            " (NAME = N'" + name + "', FILENAME = N'C:\\Program Files\\Microsoft SQL Server\\MSSQL14.MSSQLSERVER\\MSSQL\\DATA\\" + name + ".mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )" +
            " LOG ON" +
            " (NAME = N'" + name + "_log', FILENAME = N'C:\\Program Files\\Microsoft SQL Server\\MSSQL14.MSSQLSERVER\\MSSQL\\DATA\\" + name + "_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )";
            cellType = "USE[" + name + "]" +
            " SET ANSI_NULLS ON  " +
            " SET QUOTED_IDENTIFIER ON  " +
            " CREATE TABLE[dbo].[Cell_Types](    [Id_Cell_Type][int] IDENTITY(1, 1) NOT NULL, [Name] [varchar] (50) NOT NULL, CONSTRAINT[PK_Cell_Types] PRIMARY KEY CLUSTERED" +
            " ([Id_Cell_Type] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY] ";
            cells = "USE [" + name + "] " +
            " SET ANSI_NULLS ON  SET QUOTED_IDENTIFIER ON " +
            " CREATE TABLE[dbo].[Cells] (     [Id_Cell][int] IDENTITY(1,1) NOT NULL,  [Id_Cell_Type] [int] NOT NULL,  [Name] [varchar] (50) NOT NULL," +
            " [Description] [varchar] (400) NOT NULL, CONSTRAINT[PK_Cells] PRIMARY KEY CLUSTERED([Id_Cell] ASC" +
            " )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] ) ON[PRIMARY] " +
            " ALTER TABLE[dbo].[Cells] WITH CHECK ADD CONSTRAINT[FK_Cells_Cell_Types] FOREIGN KEY([Id_Cell_Type])" +
            " REFERENCES[dbo].[Cell_Types] ([Id_Cell_Type])  ALTER TABLE[dbo].[Cells] CHECK CONSTRAINT[FK_Cells_Cell_Types] ";
            cellularStructure = "USE [" + name + "]  SET ANSI_NULLS ON  SET QUOTED_IDENTIFIER ON " +
            " CREATE TABLE[dbo].[Cellular_Structures] (   [Id_Cellular_Structure][int] IDENTITY(1,1) NOT NULL," +
             "  [Description] [varchar] (400) NOT NULL, CONSTRAINT[PK_Cellular_Structures] PRIMARY KEY CLUSTERED" +
            " ([Id_Cellular_Structure] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY] ";
            cc = "USE [" + name + "]  SET ANSI_NULLS ON  SET QUOTED_IDENTIFIER ON  CREATE TABLE [dbo].[CellStructures_Cells]( 	[Id_CellStructure] [int] NOT NULL, " +
            "    [Id_Cell][int] NOT NULL ) ON[PRIMARY]  ALTER TABLE[dbo].[CellStructures_Cells]  WITH CHECK ADD CONSTRAINT[FK_CellStructures_Cells_Cells] FOREIGN KEY([Id_Cell]) " +
            " REFERENCES[dbo].[Cells]([Id_Cell])  ALTER TABLE[dbo].[CellStructures_Cells] CHECK CONSTRAINT[FK_CellStructures_Cells_Cells]  " +
            " ALTER TABLE[dbo].[CellStructures_Cells]  WITH CHECK ADD CONSTRAINT[FK_CellStructures_Cells_Cellular_Structures] FOREIGN KEY([Id_CellStructure])" +
            " REFERENCES[dbo].[Cellular_Structures]([Id_Cellular_Structure])  ALTER TABLE[dbo].[CellStructures_Cells] CHECK CONSTRAINT[FK_CellStructures_Cells_Cellular_Structures] ";
            materials = "USE [" + name + "]  SET ANSI_NULLS ON  SET QUOTED_IDENTIFIER ON  CREATE TABLE [dbo].[Materials]( 	[Id_Material] [int] IDENTITY(1,1) NOT NULL," +
            "    [Name][varchar](50) NOT NULL,  [Description] [varchar] (400) NOT NULL, CONSTRAINT[PK_Materials] PRIMARY KEY CLUSTERED" +
            " ([Id_Material] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY] ";
            details = "USE [" + name + "]  SET ANSI_NULLS ON  SET QUOTED_IDENTIFIER ON  CREATE TABLE [dbo].[Details]( 	[Id_Detail] [int] IDENTITY(1,1) NOT NULL, 	[Id_Material] [int] NOT NULL, 	[Id_CellStructure] [int] NOT NULL," +
            "    [Name][varchar](50) NOT NULL,  [Description] [varchar] (400) NOT NULL, CONSTRAINT[PK_Details] PRIMARY KEY CLUSTERED([Id_Detail] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]" +
            " ) ON[PRIMARY]  ALTER TABLE[dbo].[Details] WITH CHECK ADD CONSTRAINT[FK_Details_Cellular_Structures] FOREIGN KEY([Id_CellStructure]) REFERENCES[dbo].[Cellular_Structures] ([Id_Cellular_Structure])  ALTER TABLE[dbo].[Details] CHECK CONSTRAINT[FK_Details_Cellular_Structures]  " +
            " ALTER TABLE[dbo].[Details] WITH CHECK ADD CONSTRAINT[FK_Details_Materials] FOREIGN KEY([Id_Material]) REFERENCES[dbo].[Materials] ([Id_Material])  ALTER TABLE[dbo].[Details] CHECK CONSTRAINT[FK_Details_Materials] ";
            result = "USE [" + name + "]  SET ANSI_NULLS ON  SET QUOTED_IDENTIFIER ON  CREATE TABLE [dbo].[Results_of_research]( 	[Id_Result] [int] IDENTITY(1,1) NOT NULL, 	[Id_Detail] [int] NOT NULL, 	[Name] [varchar](50) NOT NULL, " +
            "    [Type][varchar](50) NOT NULL,  [Min_Value] [float] NOT NULL,   [Max_Value] [float] NOT NULL, CONSTRAINT[PK_Results_of_research] PRIMARY KEY CLUSTERED([Id_Result] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] " +
            ") ON[PRIMARY]  ALTER TABLE[dbo].[Results_of_research] WITH CHECK ADD CONSTRAINT[FK_Results_of_research_Details] FOREIGN KEY([Id_Detail]) REFERENCES[dbo].[Details] ([Id_Detail])  ALTER TABLE[dbo].[Results_of_research] CHECK CONSTRAINT[FK_Results_of_research_Details] ";
            constant = "USE [" + name + "]  SET ANSI_NULLS ON  SET QUOTED_IDENTIFIER ON   CREATE TABLE [dbo].[Constants]( 	[Id_Constant] [int] IDENTITY(1,1) NOT NULL, 	[Name] [varchar](50) NOT NULL, 	[Value] [float] NOT NULL, 	[Unit] [varchar](20) NULL,  CONSTRAINT [PK_Constants] PRIMARY KEY CLUSTERED " +
            " (   [Id_Constant] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] ) ON[PRIMARY] ";
            parentType = "USE[" + name + "] SET ANSI_NULLS ON SET QUOTED_IDENTIFIER ON CREATE TABLE[dbo].[ParentTypes] (     [Id_Type][int] IDENTITY(1,1) NOT NULL, [Name] [varchar] (50) NOT NULL, " +
            "CONSTRAINT[PK_ParentTypes] PRIMARY KEY CLUSTERED([Id_Type] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] ) ON[PRIMARY]";
            properties = "USE [" + name + "]  SET ANSI_NULLS ON  SET QUOTED_IDENTIFIER ON  CREATE TABLE [dbo].[Properties]( [Id_Property] [int] IDENTITY(1,1) NOT NULL, 	[Name] [varchar](50) NOT NULL, 	[Unit] [varchar](20) NOT NULL, 	[PropertyType] [int] NOT NULL,  CONSTRAINT [PK_Details_Characteristic] PRIMARY KEY CLUSTERED " +
            "(   [Id_Property] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] ) ON[PRIMARY]  ALTER TABLE[dbo].[Properties]  WITH CHECK ADD CONSTRAINT[FK_Properties_ParentTypes] FOREIGN KEY([PropertyType]) REFERENCES[dbo].[ParentTypes]([Id_Type])  ALTER TABLE[dbo].[Properties] CHECK CONSTRAINT[FK_Properties_ParentTypes] ";
            values = "USE [" + name + "]  SET ANSI_NULLS ON  SET QUOTED_IDENTIFIER ON  CREATE TABLE [dbo].[Values]( 	[Id_Value] [int] IDENTITY(1,1) NOT NULL, 	[Id_Property] [int] NOT NULL, 	[Id_Parent] [int] NOT NULL, " +
            "    [Value][float] NOT NULL,   [Id_ParentType] [int] NOT NULL, CONSTRAINT[PK_Cell_Parameter_Values] PRIMARY KEY CLUSTERED([Id_Value] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] ) ON[PRIMARY]  " +
            " ALTER TABLE[dbo].[Values] WITH CHECK ADD CONSTRAINT[FK_Values_ParentTypes] FOREIGN KEY([Id_ParentType]) REFERENCES[dbo].[ParentTypes] ([Id_Type])  ALTER TABLE[dbo].[Values] CHECK CONSTRAINT[FK_Values_ParentTypes]  ALTER TABLE[dbo].[Values] WITH CHECK ADD CONSTRAINT[FK_Values_Properties] FOREIGN KEY([Id_Property]) " +
            " REFERENCES[dbo].[Properties] ([Id_Property])  ALTER TABLE[dbo].[Values] CHECK CONSTRAINT[FK_Values_Properties] ";
            prt = "USE [" + name + "] INSERT INTO ParentTypes([Name]) VALUES ('Detail'), ('Material'), ('Cell'), ('CelluralStructure');";
            insertCS = "CREATE PROCEDURE[dbo].[InsertCS]  @descCS varchar(400),    @idCS int OUTPUT AS BEGIN    SET NOCOUNT ON; INSERT INTO Cellular_Structures([Description])    VALUES(@descCS); SET @idCS = @@IDENTITY END";
            insertDetail = "CREATE PROCEDURE [dbo].[InsertDetail] @idMat int, @idCS int, @nameDetail varchar(50), @descDet varchar(400), @idDetail int OUTPUT " +
            " AS BEGIN SET NOCOUNT ON; INSERT INTO Details(Id_Material, Id_CellStructure, [Name], [Description]) VALUES(@idMat, @idCS, @nameDetail, @descDet); SET @idDetail = @@IDENTITY END";
        }


        public String getCreateDBSting()
        {
            return this.dbCreate;
        }

        public List<String> getList()
        {
            List<String> list = new List<String>();
            list.Add(cellType);
            list.Add(cells);
            list.Add(cellularStructure);
            list.Add(cc);
            list.Add(materials);
            list.Add(details);
            list.Add(result);
            list.Add(constant);
            list.Add(parentType);
            list.Add(properties);       
            list.Add(values);
            list.Add(prt);
            return list;
        }

        public List<String> getListProc()
        {
            List<String> list = new List<String>();
          
            list.Add(insertCS);
            list.Add(insertDetail);

            return list;
        }

    }

}
