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
    public partial class Form9 : Form
    {
        mangement_storesEntities Ent;
        public Form9()
        {
            InitializeComponent();
            Ent = new mangement_storesEntities();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var store= (from d in Ent.Stores
                                 where d.store_name == comboBox4.SelectedItem.ToString()
                                 select d.store_id).First();
            var storesRecivItems= from d in Ent.DismissalNotices
                         where d.dismissalStore_id == store
                         select d;
            var StoreSendItems= from d in Ent.SupplyPermissions
                                where d.permStore_id == store
                              select d;

            listBox1.Items.Add("Code" + "          " +" Quantity");
            foreach (var d in storesRecivItems)
            {
                int count =
                    (from v in Ent.permitionItems
                    where v.code== d.Code
                    select d).Count();
                listBox1.Items.Add(d.Code+"       "+count.ToString());
            }
            foreach (var d in StoreSendItems)
            {
                listBox1.Items.Add(d.code);
                listBox1.Items.Add(d.quantity);
            }
            
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            foreach (var d in Ent.Stores)
            {
                comboBox4.Items.Add(d.store_name);
            }
           
            for (int i = 1; i <= 30; i++)
            {
              
                comboBox1.Items.Add(i.ToString());
                comboBox5.Items.Add(i.ToString());
            }
            for (int i = 1; i <= 12; i++)
            {
              
                comboBox2.Items.Add(i.ToString());
                comboBox6.Items.Add(i.ToString());
            }
            for (int i = 2021; i <= 10; i++)
            {
              
                comboBox3.Items.Add(i.ToString());
                comboBox3.Items.Add(i.ToString());
            }
        }
    }
}
