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
    public partial class Form7 : Form
    {
        mangement_storesEntities Ent;
        public Form7()
        {
            InitializeComponent();
            Ent = new mangement_storesEntities();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            foreach (var d in Ent.Clients)
            {
                comboBox1.Items.Add(d.client_name);
            }
            foreach (var d in Ent.Stores)
            {
                comboBox2.Items.Add(d.store_name);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get Stores
            var storeID = (from d in Ent.Stores
                           where d.store_name == comboBox2.SelectedItem.ToString()
                           select d.store_id).First();
            // get Employess in Stores
            var emps = from d in Ent.Users
                       where d.store_id == storeID
                       select d;
            foreach (var d in emps)
            {
                comboBox3.Items.Add(d.user_name);
            }
            // get available Item in this Stores
            /*var avilableItem = (from d in Ent.permitionItems
                       where d.Store_Id == storeID
                       select d.code).Count();
            if (avilableItem == 1)
                comboBox4.Items.Add(avilableItem);
            else
            {*/


                var avilableItem2 = (from d in Ent.permitionItems
                                    where d.Store_Id == storeID
                                    select d.code).Distinct();
                foreach (var d in avilableItem2)
                {
                    comboBox4.Items.Add(d);
                }
            
          
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DismissalNotice dismissalNotice = new DismissalNotice();
            permitionItem permitionItem = new permitionItem();
            string itemCode = comboBox4.SelectedItem.ToString();
            int quantity = int.Parse(textBox2.Text);
            int totalPrice = 0;
            //get Store ID
            var StoreId = (from d in Ent.Stores
                          
                           where d.store_name ==comboBox2.SelectedItem.ToString()
                           select d.store_id).First();
            //get department
            var DeptId = (from d in Ent.Stores

                           where d.store_name == comboBox2.SelectedItem.ToString()
                           select d.department_id).First();
            //get Client Id
            var ClientId = (from d in Ent.Clients

                          where d.client_name == comboBox1.SelectedItem.ToString()
                          select d.client_id).First();

            //GET avilable 
            var availableQuantity = (from d in Ent.permitionItems
                                     orderby d.ProductionEnd.Year
                                     where d.code == itemCode
                                     select d).Count();
            //Get Employee ID
            var empID = (from d in Ent.Users

                          where d.user_name == comboBox3.SelectedItem.ToString()
                          select d.user_id).First();

            //get date
            DateTime date = new DateTime();
            //Check there are Vailable Quantity
            if (quantity < availableQuantity)
            {
                //delete items
                for (int i = 0; i < quantity; i++)

                {
                    permitionItem = (from d in Ent.permitionItems
                                     orderby d.ProductionEnd
                                     where d.code == itemCode && d.Store_Id == StoreId
                                     select d).First();
                    totalPrice += permitionItem.item_price;
                    Ent.permitionItems.Remove(permitionItem);
                    Ent.SaveChanges();
                }
                //add information About permition 
                dismissalNotice.dismissal_id = int.Parse(textBox1.Text);
               
                dismissalNotice.dismissal_date = date;
                dismissalNotice.dismissalDept_id = DeptId;
                dismissalNotice.dismissalUser_id = empID;
                dismissalNotice.dismissalStore_id = StoreId;
                dismissalNotice.Code = itemCode;
                dismissalNotice.Quantity = quantity;
                dismissalNotice.totalPrice = totalPrice;
                dismissalNotice.dismissalClient_id = ClientId;
                Ent.DismissalNotices.Add(dismissalNotice);
                Ent.SaveChanges();
                MessageBox.Show("transform successful");
            }
            else
            {
                MessageBox.Show("Quantity is not Avilable");
            }

          
            
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime now = new DateTime();
            textBox3.Text = now.ToString();
        }
    }
}
