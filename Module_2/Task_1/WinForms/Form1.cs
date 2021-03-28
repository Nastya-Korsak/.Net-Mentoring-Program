using System;
using System.Windows.Forms;

namespace Task1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var name = this.textBox1.Text;

            MessageBox.Show($"Hello {name}!");
        }
    }
}
