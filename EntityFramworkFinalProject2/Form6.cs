using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityFramworkFinalProject2
{
    public partial class Form6 : Form
    {
        mangement_storesEntities Ent;

        public Form6()
        {
            InitializeComponent();
            Ent = new mangement_storesEntities();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            foreach (var d in Ent.C_itemsDetails)
            {
                comboBox1.Items.Add(d.item_code);
            }
            foreach (var d in Ent.Units)
            {
                comboBox2.Items.Add(d.unit_name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //get unit id
            var unit_id = (from d in Ent.Units
                          where d.unit_name == comboBox2.SelectedItem.ToString()
                          select d.unit_id).First();
            string Unit_Code = textBox1.Text;
            C_itemsDetails itemsDetails = new C_itemsDetails();
            itemsDetails = Ent.C_itemsDetails.Find(textBox1.Text);
            if (itemsDetails == null)
            {
                C_itemsDetails itemsDetails1 = new C_itemsDetails();
                itemsDetails1.itemUnit_id = unit_id;
                itemsDetails1.item_code = Unit_Code;
                itemsDetails1.item_name = textBox2.Text;
                Ent.C_itemsDetails.Add(itemsDetails1);
                Ent.SaveChanges();
                textBox1.Text = textBox2.Text = string.Empty;
                
            }
            else
            {
                MessageBox.Show("this Code Found");
                textBox1.Text = string.Empty;
            }
            comboBox1.Items.Clear();
            foreach (var d in Ent.C_itemsDetails)
            {
                comboBox1.Items.Add(d.item_code);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string Unit_Code = comboBox1.SelectedItem.ToString();
            C_itemsDetails itemsDetails = new C_itemsDetails();
            itemsDetails = Ent.C_itemsDetails.Find(Unit_Code);
           
                Ent.C_itemsDetails.Remove(itemsDetails);
                Ent.SaveChanges();
            textBox1.Text = textBox2.Text = string.Empty;
            MessageBox.Show("Deleted is Successfull");
            comboBox1.Items.Clear();
            foreach (var d in Ent.C_itemsDetails)
            {
                comboBox1.Items.Add(d.item_code);
            }
           
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //get unit id
            var unit_id = (from d in Ent.Units
                           where d.unit_name == comboBox2.SelectedItem.ToString()
                           select d.unit_id).First();
            string Unit_Code = comboBox1.SelectedItem.ToString();
            C_itemsDetails itemsDetails = new C_itemsDetails();
            itemsDetails = Ent.C_itemsDetails.Find(Unit_Code);
            if (itemsDetails != null)
            {
                


                itemsDetails.itemUnit_id = unit_id;
                itemsDetails.item_code = Unit_Code;
                itemsDetails.item_name = textBox2.Text;
            
                Ent.SaveChanges();
                textBox1.Text = textBox2.Text = string.Empty;

            }
            else
            {
                MessageBox.Show("this Code Not Found");
                textBox1.Text = string.Empty;
            }
            foreach (var d in Ent.C_itemsDetails)
            {
                comboBox1.Items.Add(d.item_code);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (from d in Ent.C_itemsDetails
                        where d.item_code == comboBox1.SelectedItem.ToString()
                        select d).First();
            textBox2.Text = item.item_name;
        }
    }
}
