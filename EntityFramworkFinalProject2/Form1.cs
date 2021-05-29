using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace EntityFramworkFinalProject2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 frm = new Form4();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form5 frm = new Form5();
            frm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form6 frm = new Form6();
            frm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form7 frm = new Form7();
            frm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form8 frm = new Form8();
            frm.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (Thread.CurrentThread.CurrentUICulture.IetfLanguageTag)
            {
                case ("ar-EG"):
                    Thread.CurrentThread.CurrentUICulture =
                        new System.Globalization.CultureInfo("en-US"); break;
                case "en-US":
                    Thread.CurrentThread.CurrentUICulture =
               new System.Globalization.CultureInfo("ar-EG"); break;


            }
            this.Controls.Clear();
            InitializeComponent();
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            switch (Thread.CurrentThread.CurrentUICulture.IetfLanguageTag)
            {
                case ("ar-EG"):
                    Thread.CurrentThread.CurrentUICulture =
                        new System.Globalization.CultureInfo("en-US"); break;
                case "en-US":
                    Thread.CurrentThread.CurrentUICulture =
               new System.Globalization.CultureInfo("ar-EG"); break;


            }
            this.Controls.Clear();
            InitializeComponent();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            switch (Thread.CurrentThread.CurrentUICulture.IetfLanguageTag)
            {
                case ("ar-EG"):
                    Thread.CurrentThread.CurrentUICulture =
                        new System.Globalization.CultureInfo("en-US"); break;
                case "en-US":
                    Thread.CurrentThread.CurrentUICulture =
               new System.Globalization.CultureInfo("ar-EG"); break;


            }
            this.Controls.Clear();
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form9 frm = new Form9();
            frm.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form10 frm = new Form10();
            frm.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form11 frm = new Form11();
            frm.Show();
        }
    }
}
