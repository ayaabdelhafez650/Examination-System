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
    public partial class Form10 : Form
    {
        mangement_storesEntities Ent;
        public Form10()
        {
            InitializeComponent();
            Ent = new mangement_storesEntities();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var expireItemNearly = from d in Ent.permitionItems
                                     orderby d.ProductionEnd .Year

                                   select d;
            listBox1.Items.Add("Code" + "          " +"Production"+"             " +"Price"+ "     " + "Store_Id");
            foreach (var d in expireItemNearly)
            {
                listBox1.Items.Add(d.code +"      "+d.ProductionEnd+"      "+ d.item_price +"     "+ d.Store_Id);
            }
        }
    }
}
