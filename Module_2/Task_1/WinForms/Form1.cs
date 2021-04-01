using System;
using System.Windows.Forms;
using HelloLibrary;

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
            var name = textBox1.Text;

            var printLine = WelcomeMessage.GetWelcomeMessage(name);

            MessageBox.Show(printLine);
        }
    }
}
