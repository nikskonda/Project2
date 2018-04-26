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
    public partial class Form2 : Form
    {
        DataSetWorker d = new DataSetWorker("NIKITA", "Project2");

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           
            d.sort(dataGridView1, dataGridView2);

            d.filter(dataGridView3, textBox1.Text, textBox2.Text);

            d.calc(dataGridView4);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            d.filter(dataGridView3, textBox1.Text, textBox2.Text);
        }
    }

}
