using cells_db.model;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cube
{
    public partial class Form3 : Form
    {
        private int id = 0;
        private string Name = "";
        public Form3(int id, string name)
        {
            this.id = id;
            this.Name = name;
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'Project2DataSet.ResearchResult' table. You can move, or remove it, as needed.

            int count;

            string head;

            if (id == 0)
            {
                this.ResearchResultTableAdapter.Fill(this.Project2DataSet.ResearchResult);
                count = Project2DataSet.ResearchResult.Count;
                ResearchResultBindingSource.DataSource = Project2DataSet.ResearchResult;
                head = "Result of research of details:";
            } else
            {
                this.ResearchResultTableAdapter.Fill(this.Project2DataSet.ResearchResult);
                count = Project2DataSet.ResearchResult.ToList().FindAll(x => x.Id_Detail == id).Count;
                head = "Result of research of" + this.Name + ":";
                ResearchResultBindingSource.DataSource = Project2DataSet.ResearchResult.ToList().FindAll(x => x.Id_Detail == id);
            }

            ReportParameter rp = new ReportParameter("count", count+"");
            ReportParameter rp1 = new ReportParameter("Head", head.ToUpper());
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[]{rp, rp1});
            this.reportViewer1.RefreshReport();
        }
    }
}
