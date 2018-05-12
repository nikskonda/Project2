﻿using System;
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


        public DBConnectionPage()
        {
            InitializeComponent();       
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
                DialogManager.showDialogError("Empty Field.");
                return;
            }

            INIManager ini = new INIManager(iniName);
            ini.WritePrivateString("Enter", "ServerName", textBox1.Text);
            ini.WritePrivateString("Enter", "DataBaseName", textBox2.Text);
            ini.WritePrivateString("Enter", "Path", folderBrowserDialog1.SelectedPath);

            if (SQLWorker.IsCreate(textBox1.Text, textBox2.Text))
                {
                    DataViewPage form1 = new DataViewPage(textBox1.Text, textBox2.Text);
                    form1.Show();
                }
                else
                {
                    if (DialogManager.showDialogYesNo("Data Base or Server not found. Create new Data Base?"))
                    {
                        if (!creater())
                        {
                            DialogManager.showDialogError("Error. Try Again.");
                            if (!SQLWorker.delDB(textBox1.Text, textBox2.Text))
                            {
                                DialogManager.showDialogError("ой-ой-ой. Это очень-очень плохо");
                            }
                            return;
                    }
                    else
                    {
                        DataViewPage form1 = new DataViewPage(textBox1.Text, textBox2.Text);
                        form1.Show();
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
