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
    public partial class Form5 : Form
    {
        mangement_storesEntities Ent;
        public Form5()
        {
            InitializeComponent();
            Ent = new mangement_storesEntities();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            foreach (var d in Ent.Departments)
            {
                comboBox1.Items.Add(d.dept_name);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Store st = new Store();
            int store_id = int.Parse(textBox1.Text);
            st = Ent.Stores.Find(store_id);
            var dept = (from d in Ent.Departments
                       where d.dept_name == comboBox1.SelectedItem.ToString()
                       select d.dep_id).First();
            var emp= (from d in Ent.Users
                       where d.user_name == comboBox2.SelectedItem.ToString()
                       select d.user_id).First();
            if (st == null)
            {
                Store store = new Store();
                store.store_id = int.Parse(textBox1.Text);
                store.store_name = textBox2.Text;
                store.department_id = dept;
                store.person_id = emp;
                Ent.Stores.Add(store);

                Ent.SaveChanges();

            }

            else
            {
                MessageBox.Show("Store  found");
            }

            textBox2.Text = textBox1.Text = string.Empty;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Department dept = new Department();
           
            string deptNameSelect = comboBox1.SelectedItem.ToString();
             dept = (from d in Ent.Departments
                    where d.dept_name == deptNameSelect
                    select d).First();
            var emps = from d in Ent.Users
                       where d.dept_id == dept.dep_id
                       select d;
            foreach (var d in emps)
            {
                comboBox2.Items.Add(d.user_name);
            }
            foreach (var d in Ent.Stores)
            {
                comboBox3.Items.Add(d.store_name);
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Store st = new Store();
            int store_id = int.Parse(textBox1.Text);

            st = Ent.Stores.Find(store_id);
            if (st != null)
            {
                Ent.Stores.Remove(st);


                Ent.SaveChanges();

            }
            else
            {
                MessageBox.Show("Store not found");
            }
            textBox1.Text = string.Empty;
            foreach (var d in Ent.Stores)
            {
                comboBox3.Items.Add(d);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Store st = new Store();

            int store_id = (from d in Ent.Stores
                            where d.store_name == comboBox3.SelectedItem.ToString()
                            select d.store_id).First();
            var emps = (from d in Ent.Users
                       where d.user_name == comboBox2.SelectedItem.ToString()
                       select d.user_id).First();
            var dept = (from d in Ent.Departments
                        where d.dept_name == comboBox1.SelectedItem.ToString()
                        select d.dep_id).First();

            st = Ent.Stores.Find(store_id);
            if (st != null)
            {
                st.store_id = int.Parse(textBox1.Text);
                st.store_name = textBox2.Text;
                st.person_id = emps;
                st.department_id = dept;

                Ent.SaveChanges();

            }
            else
            {
                MessageBox.Show("Store not found");
            }
            textBox2.Text = textBox1.Text = string.Empty;
            foreach (var d in Ent.Stores)
            {
                comboBox3.Items.Add(d.store_name);   
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var store = (from d in Ent.Stores
                            where d.store_name == comboBox3.SelectedItem.ToString()
                            select d).First();

            textBox1.Text = store.store_id.ToString();
            textBox2.Text = store.store_name.ToString();

        }
    }
}
