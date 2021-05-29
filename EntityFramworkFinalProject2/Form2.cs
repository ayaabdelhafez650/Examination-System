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
    public partial class Form2 : Form
    {
        mangement_storesEntities Ent;
        public Form2()
        {
            InitializeComponent();
            Ent = new mangement_storesEntities();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            foreach (var d in Ent.Stores)
            {
                comboBox2.Items.Add(d.store_name);
            }
            //select production date
            int day = 0, mounth = 0, year = 2020;
            for (int i = 0; i < 30; i++)
            {
                day += 1;
                comboBox4.Items.Add(day.ToString());
            }
            for (int i = 0; i < 12; i++)
            {
                mounth += 1;
                comboBox5.Items.Add(mounth.ToString());
            }
            for (int i = 0; i < 50; i++)
            {
                year += 1;
                comboBox6.Items.Add(year.ToString());
            }
            //select validity period
            comboBox7.Items.Add("Mounth");
            comboBox7.Items.Add("Year");
            // add supplier
            foreach (var d in Ent.Suppliers)
            {
                comboBox1.Items.Add(d.supplier_name);
            }
            var code_items = (from s in Ent.C_itemsDetails
                              select s.item_code).Distinct();
            foreach (var d in code_items)
            {
             
                comboBox3.Items.Add(d);
            }
            comboBox3.Items.Add("1000");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        { 
            string StoreNameSelect = comboBox2.SelectedItem.ToString();
            Store store1 = new Store();

            store1 = (from d in Ent.Stores
                      where d.store_name == StoreNameSelect
                      select d).First();
            var emps = from d in Ent.Users
                       where d.store_id == store1.store_id
                       select d;

            foreach (var d in emps)
            {
                comboBox8.Items.Add(d.user_name);
            }
            //get department number
            int dept_id = store1.department_id;
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;
            DateTime now = DateTime.Now;
            textBox2.Text = now.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            permitionItem pitm = new permitionItem();
            SupplyPermission sup = new SupplyPermission();
            int permition_ID = int.Parse(textBox1.Text);
            //get employee ID
            int emp = (from em in Ent.Users
                       where em.user_name == comboBox8.SelectedItem.ToString()
                       select em.user_id).First();


            DateTime dateTime = DateTime.Now;
            //get Supplier ID
            int supplier = (from em in Ent.Suppliers
                            where em.supplier_name == comboBox1.SelectedItem.ToString()
                            select em.supplier_id).First();
            //item Code
            string itemCode = comboBox3.SelectedItem.ToString();
            // Item Quantity
            int quantity = int.Parse(textBox5.Text);
            //production date
            int day = int.Parse(comboBox4.SelectedItem.ToString());
            int mounth = int.Parse(comboBox5.SelectedItem.ToString());
            int year = int.Parse(comboBox6.SelectedItem.ToString());

            string date = day.ToString() + "/" + mounth.ToString() + "/" + year.ToString();
            DateTime production_data = DateTime.Parse(date);
            // period of Validity

            int peroidvalidity = int.Parse(textBox4.Text);
            //string peroidvaliditytype = comboBox1.SelectedItem.ToString();
            // end date
            




            //perice
            int price = int.Parse(textBox3.Text);
            //get unit name
            int unitId = (from d in Ent.C_itemsDetails
                          where d.item_code==comboBox3.SelectedItem.ToString()
                          select d.itemUnit_id).First();
            var unitName = from d in Ent.Units
                           where d.unit_id == unitId
                           select d.unit_name;




            C_itemsDetails item = new C_itemsDetails();
            //item = Ent.Items.Find(itemCode);

            string StoreNameSelect = comboBox2.SelectedItem.ToString();
            // store1 = new Store();

            var store1 = (from d in Ent.Stores
                          where d.store_name == StoreNameSelect
                          select d.store_id).First();
            /*  var permitionItem = from d in Ent.permitionItems
                                  orderby d.ProductionEnd
                                  where d.Store_Id == store1
                                  select d*/
            var dept = (from d in Ent.Stores
                          where d.store_name == StoreNameSelect
                          select d.department_id).First();
            DateTime endDate;
            if (comboBox7.SelectedIndex == 0)
            {
              endDate = production_data.AddMonths(peroidvalidity);
            }
            else
            {
              endDate = production_data.AddYears(peroidvalidity);
            }

            SupplyPermission supplyPermission = new SupplyPermission();
            supplyPermission.permission_id = int.Parse(textBox1.Text);
            DateTime now = new DateTime();
            supplyPermission.permission_date = now;
            supplyPermission.permSupplier_id = supplier;
            supplyPermission.permStore_id = store1;
            supplyPermission.permUser_id = emp;
            supplyPermission.permDept_id = dept;
            supplyPermission.code = itemCode;
            supplyPermission.quantity = quantity;
            supplyPermission.totalprice = quantity * price;
            Ent.SupplyPermissions.Add(supplyPermission);
            Ent.SaveChanges();
            permitionItem permitionItem = new permitionItem();
            for (int i = 0;i< quantity; i++)
            {
                permitionItem.permition_id = int.Parse(textBox1.Text);
                permitionItem.item_date = production_data;

                permitionItem.item_price = price;
                permitionItem.code = itemCode;
                permitionItem.Store_Id = store1;
                permitionItem.ProductionEnd = endDate;
                permitionItem.item_periodValidity = peroidvalidity;
                Ent.permitionItems.Add(permitionItem);
                Ent.SaveChanges();

            }

            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text
                = textBox5.Text = string.Empty;




        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;
            DateTime now = DateTime.Now;
            textBox2.Text = now.ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
