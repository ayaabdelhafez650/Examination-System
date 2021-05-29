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
    public partial class Form8 : Form
    {
        mangement_storesEntities Ent;
        public Form8()
        {
            InitializeComponent();
            Ent = new mangement_storesEntities();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            foreach (var d in Ent.Stores)
            {
                comboBox1.Items.Add(d.store_name);
                comboBox2.Items.Add(d.store_name);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var storeID = (from d in Ent.Stores
                           where d.store_name == comboBox1.SelectedItem.ToString()
                           select d.store_id).First();
            
          var  employess = from d in Ent.Users
                        where d.store_id == storeID
                        select d;
            foreach (var d in employess)
            {
                comboBox3.Items.Add(d.user_name);
            }
            var ItemCode = (from d in Ent.permitionItems
                           where d.Store_Id == storeID
                           select d.code).Distinct();
            foreach (var d in ItemCode)
            {
                comboBox5.Items.Add(d);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var storeID = (from d in Ent.Stores
                           where d.store_name == comboBox2.SelectedItem.ToString()
                           select d.store_id).First();

            var employess = from d in Ent.Users
                            where d.store_id == storeID
                            select d;
            foreach (var d in employess)
            {
                comboBox4.Items.Add(d.user_name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { //Store from
            int quntity = int.Parse(textBox1.Text);
            var storeID = (from d in Ent.Stores
                           where d.store_name == comboBox1.SelectedItem.ToString()
                           select d.store_id).First();
           

            var employeSend = (from d in Ent.Users
                            where d.store_id == storeID
                            select d.user_id).First();
            var AvailableQuantity = (from d in Ent.permitionItems
                            where d.Store_Id == storeID
                            select d.code).Count();
            string itemCode = comboBox5.SelectedItem.ToString();
            //Store to
            var storeIDTo = (from d in Ent.Stores
                           where d.store_name == comboBox2.SelectedItem.ToString()
                           select d.store_id).First();
            var employeeRecive = (from d in Ent.Users
                               where d.store_id == storeIDTo
                               select d.user_id).First() ;

            permitionItem itemtransfer = new permitionItem();
            if (quntity <= AvailableQuantity)
            {
                for (int i = 0; i < quntity; i++)
                {
                  itemtransfer = (from v in Ent.permitionItems
                                    orderby v.item_date descending
                                    where v.code == itemCode && v.Store_Id == storeID
                                    select v).First();
                    itemtransfer.Store_Id = storeIDTo;
                    Ent.SaveChanges();

                }
                transactionItem transactionItem = new transactionItem();
                transactionItem.StoreSend = storeID;
                transactionItem.StoreRecive = storeIDTo;
                transactionItem.EmployeeSend =employeSend;
                transactionItem.EmployeeRecive = employeeRecive;
                transactionItem.Code = itemCode;
                DateTime d = new DateTime();

                transactionItem.Date = d;
                transactionItem.Quantity = quntity;

                Ent.transactionItems.Add(transactionItem);
                Ent.SaveChanges();

            }
            else
            {
                MessageBox.Show("Store don't have enough quantity");
            }

        }
    }
}
