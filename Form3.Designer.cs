namespace Cube
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.ResearchResultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Project2DataSet = new Cube.Project2DataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ResearchResultTableAdapter = new Cube.Project2DataSetTableAdapters.ResearchResultTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ResearchResultBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Project2DataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // ResearchResultBindingSource
            // 
            this.ResearchResultBindingSource.DataMember = "ResearchResult";
            this.ResearchResultBindingSource.DataSource = this.Project2DataSet;
            // 
            // Project2DataSet
            // 
            this.Project2DataSet.DataSetName = "Project2DataSet";
            this.Project2DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.ResearchResultBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Cube.Report1.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(800, 450);
            this.reportViewer1.TabIndex = 0;
            // 
            // ResearchResultTableAdapter
            // 
            this.ResearchResultTableAdapter.ClearBeforeFill = true;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.reportViewer1);
            this.Name = "Form3";
            this.Text = "Report";
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ResearchResultBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Project2DataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource ResearchResultBindingSource;
        private Project2DataSet Project2DataSet;
        private Project2DataSetTableAdapters.ResearchResultTableAdapter ResearchResultTableAdapter;
    }
}