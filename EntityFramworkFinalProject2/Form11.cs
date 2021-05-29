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
    public partial class Form11 : Form
    {
        mangement_storesEntities Ent;
        public Form11()
        {
            InitializeComponent();
            Ent = new mangement_storesEntities();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var StoreID = (from d in Ent.Stores
                          where d.store_name == comboBox1.SelectedItem.ToString()
                          select d.store_id).First();
            var items= from d in Ent.permitionItems
                       orderby d.permition_id
                       where d.Store_Id==StoreID
                       select d;
            foreach (var n in Ent.permitionItems)
            {
                var date= (from d in Ent.SupplyPermissions
                         
                          where d.permission_id == n.permition_id
                          select d.permission_date).First();

                listBox1.Items.Add(n.code + "                 " + date);
            }

        }

        private void Form11_Load(object sender, EventArgs e)
        {
            foreach (var d in Ent.Stores)
            {
                comboBox1.Items.Add(d.store_name);
            }
        }
    }
}
