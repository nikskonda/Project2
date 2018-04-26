using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cube
{
    class DataSetWorker
    {
        private String PCname;
        private String DBname;

        private SqlConnection sqlConnection;    

        public DataSetWorker(String PCname, String DBname)
        {
                this.PCname = PCname;
                this.DBname = DBname;
        }

        private SqlConnection GetSqlConnection()
        {
            StringBuilder sb = new StringBuilder("server = ");
            sb.Append(PCname).Append("; database = ").Append(DBname).Append("; Integrated Security = true");
            SqlConnection sql = new SqlConnection(sb.ToString());
            return sql;
        }

        public void readerFromDB(ComboBox cmb)
        {
                // Организация подключения
                SqlConnection con = GetSqlConnection();
                string command = "SELECT * FROM Details";
                SqlDataAdapter adapter = new SqlDataAdapter(command, con);

                // Заполнение tDataSet
                DataSet dataset = new DataSet();
                adapter.Fill(dataset, "Details");

                // Вывод из DataSet строк таблицы Employees в элемент list1
                foreach (DataRow row in dataset.Tables["Details"].Rows)
                {
                    string result = row["Name"].ToString();
                    cmb.Items.Add(result);
                }

        }

        public void creater(ComboBox cmb)
        {
            DataSet store = new DataSet("Project2");
            DataTable table = new DataTable("Details");
            
            store.Tables.Add(table);

            
            DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            idColumn.Unique = true; // столбец будет иметь уникальное значение
            idColumn.AllowDBNull = false; // не может принимать null
            idColumn.AutoIncrement = true; // будет автоинкрементироваться
            idColumn.AutoIncrementSeed = 1; // начальное значение
            idColumn.AutoIncrementStep = 1; // приращении при добавлении новой строки

            DataColumn nameColumn = new DataColumn("Name", Type.GetType("System.String"));
            DataColumn priceColumn = new DataColumn("Description", Type.GetType("System.String"));
            
            table.Columns.Add(idColumn);
            table.Columns.Add(nameColumn);
            table.Columns.Add(priceColumn);

            // определение первичного ключа таблицы books
            table.PrimaryKey = new DataColumn[] { table.Columns["Id"] };

            DataRow row = table.NewRow();
            table.Rows.Add(new object[] { null, "Крышка", "Крышка 1" }); 
            table.Rows.Add(new object[] { null, "Подставка", "Подставка 1" });

            foreach (DataRow row1 in table.Rows)
            {
                string result = row1["Name"].ToString();
                cmb.Items.Add(result);
            }

        }


        public void updater()
        {
            SqlConnection con = GetSqlConnection();
            string command = "SELECT * FROM Details";
            SqlDataAdapter adapter = new SqlDataAdapter(command, con);

            DataSet dataset = new DataSet();
            adapter.Fill(dataset, "Details");    

            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            DataTable table = dataset.Tables["Details"];
            table.Rows.Add(new object[] { null, "Крышка", "Крышка 1" });
            table.Rows.Add(new object[] { null, "Подставка", "Подставка 1" });
            adapter.Update(dataset, "Details");
        }

        public void sort(DataGridView data1, DataGridView data2)
        {
            SqlConnection con = GetSqlConnection();
            string command = "SELECT * FROM Details";
            SqlDataAdapter adapter = new SqlDataAdapter(command, con);

            DataSet dataset = new DataSet();
            adapter.Fill(dataset, "Details");


            // Сортировка по фамилии и привязка к элементу GridView2
            DataView view2 = new DataView(dataset.Tables["Details"]);
            view2.Sort = "Name";
            data1.DataSource = view2;

            // Сортировка no имени и привязка к элементу GridView3
            DataView view3 = new DataView(dataset.Tables["Details"]);
            view3.Sort = "Id_Detail";
            data2.DataSource = view3;
        }

        public void filter(DataGridView data, String id, String name)
        {
            SqlConnection con = GetSqlConnection();
            string command = "SELECT * FROM Details";
            SqlDataAdapter adapter = new SqlDataAdapter(command, con);

            DataSet dataset = new DataSet();
            adapter.Fill(dataset, "Details");

            if (!id.Equals(""))
            {
                id = "Id_Detail=" + id;
            }

            if (!name.Equals(""))
            {
                name = "Name='" + name + "'";
            }

            string res = "";
            if (!name.Equals("") && !id.Equals(""))
            {
                res = id + " and " + name;
            }

            if (name.Equals("") || id.Equals(""))
            {
                res = id + name;
            }

            if (name.Equals("") && id.Equals(""))
            {
                DataView view2 = new DataView(dataset.Tables["Details"]);
                data.DataSource = view2;
            }
            else
            {
                DataView view2 = new DataView(dataset.Tables["Details"]);
                view2.RowFilter = res;
                data.DataSource = view2;

            }
        }

        public void calc(DataGridView data)
        {
            SqlConnection con = GetSqlConnection();
            string command = "SELECT * FROM Details";
            SqlDataAdapter adapter = new SqlDataAdapter(command, con);

            DataSet dataset = new DataSet();
            adapter.Fill(dataset, "Details");

            DataColumn count = new DataColumn(
             "AVG ID", typeof(int), "AVG(Id_Detail)");
            dataset.Tables["Details"].Columns.Add(count);
            data.DataSource = dataset.Tables["Details"];
           
        }

    }
}
