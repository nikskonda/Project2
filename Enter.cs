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
    public partial class Enter : Form
    {
        public Enter()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if ( IsNull(textBox1) || IsNull(textBox2))
            {
                DialogManager.showDialogError("Empty Field");
                return;
            }

                if (SQLWorker.IsCreate(textBox1.Text, textBox2.Text))
                {
                    Form1 form1 = new Form1(textBox1.Text, textBox2.Text);
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
                        Form1 form1 = new Form1(textBox1.Text, textBox2.Text);
                        form1.Show();
                    }

                    }
                }
        }

        private bool creater()
        {
            return SQLWorker.createDataBase(textBox1.Text, textBox2.Text);
        }
    }
}
