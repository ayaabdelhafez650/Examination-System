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
    public partial class Form3 : Form
    {
        mangement_storesEntities Ent;
        public Form3()
        {
            InitializeComponent();
            Ent = new mangement_storesEntities();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            foreach (var d in Ent.Suppliers)
            {
                comboBox1.Items.Add((d.supplier_id).ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //insert new Supplier
            Supplier supplier = new Supplier();
            int supplier_id = int.Parse(textBox1.Text);
            supplier = Ent.Suppliers.Find(supplier_id);
            if (supplier == null)
            {
                Supplier supplier1 = new Supplier();
                supplier1.supplier_id = supplier_id;
                supplier1.supplier_name = textBox2.Text;
                supplier1.supplier_phone = textBox3.Text;
                supplier1.supplier_fax = textBox4.Text;
                supplier1.supplier_mail = textBox5.Text;
                supplier1.supplier_website = textBox6.Text;
                supplier1.supplier_mobile = textBox7.Text;
                Ent.Suppliers.Add(supplier1);
                Ent.SaveChanges();
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text =
               textBox5.Text = textBox6.Text = textBox7.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Supplier is found and change id");
                textBox1.Text = string.Empty;
            }
            //update the SupplierID in combbox1
            foreach (var d in Ent.Suppliers)
            {
                comboBox1.Items.Add((d.supplier_id).ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //update  Supplier
            Supplier supplier = new Supplier();
            int supplier_id = int.Parse(comboBox1.SelectedItem.ToString());
            supplier = Ent.Suppliers.Find(supplier_id);
            if (supplier != null)
            {

                supplier.supplier_id = int.Parse(comboBox1.Text);
                supplier.supplier_name = textBox2.Text;
                supplier.supplier_phone = textBox3.Text;
                supplier.supplier_fax = textBox4.Text;
                supplier.supplier_mail = textBox5.Text;
                supplier.supplier_website = textBox6.Text;
                supplier.supplier_mobile = textBox7.Text;
                Ent.SaveChanges();
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text =
               textBox5.Text = textBox6.Text = textBox7.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Supplier is found and change id");
                textBox1.Text = string.Empty;
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //delete Supplier
            Supplier supplier = new Supplier();
            int supplier_id = int.Parse(comboBox1.SelectedItem.ToString());
            supplier = Ent.Suppliers.Find(supplier_id);
            if (supplier != null)
            {

                Ent.Suppliers.Remove(supplier);
                Ent.SaveChanges();
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text =
               textBox5.Text = textBox6.Text = textBox7.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Supplier is found and change id");
                textBox1.Text = string.Empty;
            }

            textBox1.Enabled = true;
            //update the Supplier_id in combbox1
            foreach (var d in Ent.Suppliers)
            {
                comboBox1.Items.Add((d.supplier_id).ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            Supplier supplier = new Supplier();
            int supplier_id = int.Parse(comboBox1.SelectedItem.ToString());
            supplier = Ent.Suppliers.Find(supplier_id);
            textBox2.Text = supplier.supplier_name;
            textBox1.Text = supplier.supplier_id.ToString();
            textBox4.Text = supplier.supplier_fax;
            textBox5.Text = supplier.supplier_mail;
            textBox6.Text = supplier.supplier_website;
            textBox7.Text = supplier.supplier_mobile;
        }
    }
}
