
using cells_db.exception;
using cells_db.sql;
using cells_db.util;
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
    public partial class DBConnectionPage : Form
    {
        private String iniName = ".//Project2.ini";

        private ISQLWorker sqlWorker;

        public DBConnectionPage()
        {
            InitializeComponent();       
        }

        public ISQLWorker GetSQLWorker()
        {
            if (sqlWorker == null) throw new SQLWorkerException("SQLWorker is equal to null.");
            else
            {
                return sqlWorker;
            }
        }

        private void Enter_Load(object sender, EventArgs e)
        {
            INIManager ini = new INIManager(iniName);
            textBox1.Text = ini.GetPrivateString("Enter", "ServerName");
            textBox2.Text = ini.GetPrivateString("Enter", "DataBaseName");
            folderBrowserDialog1.SelectedPath = ini.GetPrivateString("Enter", "Path");
            textBox3.Text = folderBrowserDialog1.SelectedPath;
        }

        private bool IsNull(TextBox txt)
        {
            if ((txt.Text == null) || (txt.Text.Equals("")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsNull(String txt)
        {
            if ((txt == null) || (txt.Equals("")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            if ( IsNull(textBox1) || IsNull(textBox2) || IsNull(folderBrowserDialog1.SelectedPath))
            {
                DialogManager.showDialogError("Empty Fields.");
                return;
            }

            INIManager ini = new INIManager(iniName);
            ini.WritePrivateString("Enter", "ServerName", textBox1.Text);
            ini.WritePrivateString("Enter", "DataBaseName", textBox2.Text);
            ini.WritePrivateString("Enter", "Path", folderBrowserDialog1.SelectedPath);

            if (SQLWorker.IsCreate(textBox1.Text, textBox2.Text))
                {
                    sqlWorker = new SQLWorker(textBox1.Text, textBox2.Text);

                    if (checkBox1.Checked)
                    {
                        DataViewPage form1 = new DataViewPage(sqlWorker);
                        form1.Show();
                    } else
                    {
                        DialogManager.showDialogInfo("Sucsess! You can close window.");
                    }
                }
                else
                {
                    if (DialogManager.showDialogYesNo("Data Base or Server not found. Create new Data Base?"))
                    {
                        if (!creater())
                        {
                            DialogManager.showDialogError("Error creating database. Try Again.");
                            if (!SQLWorker.delDB(textBox1.Text, textBox2.Text))
                            {
                                DialogManager.showDialogError("Error cleaning data.");
                            }
                            return;
                    }
                    else
                    {
                        sqlWorker = new SQLWorker(textBox1.Text, textBox2.Text);

                        if (checkBox1.Checked)
                        {
                            DataViewPage form1 = new DataViewPage(sqlWorker);
                            form1.Show();
                        }
                        else
                        {
                            DialogManager.showDialogInfo("Sucsess! You can close window.");
                        }
                    }
                    }
                }
        }

        private bool creater()
        {
            return SQLWorker.createDataBase(textBox1.Text, textBox2.Text, folderBrowserDialog1.SelectedPath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox3.Text = folderBrowserDialog1.SelectedPath;
        }

    }
}
