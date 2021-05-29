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
    public partial class Form4 : Form
    {
        mangement_storesEntities Ent;
        public Form4()
        {
            InitializeComponent();
            Ent = new mangement_storesEntities();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            //fill client_id
            foreach (var d in Ent.Clients)
            {
                comboBox1.Items.Add((d.client_id).ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //insert new client
            Client client1 = new Client();
            int client_id = int.Parse(textBox1.Text);
            client1 = Ent.Clients.Find(client_id);
            if (client1 == null)
            {
                Client client = new Client();
                client.client_id = client_id;
                client.client_name = textBox2.Text;
                client.client_phone = textBox3.Text;
                client.client_fax = textBox4.Text;
                client.client_mail = textBox5.Text;
                client.client_website = textBox6.Text;
                client.client_mobile = textBox7.Text;
                Ent.Clients.Add(client);
                Ent.SaveChanges();
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text =
               textBox5.Text = textBox6.Text = textBox7.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Client is found and change id");
                textBox1.Text = string.Empty;
            }
            //fill client_id
            foreach (var d in Ent.Clients)
            {
                comboBox1.Items.Add((d.client_id).ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //update  client
            Client client = new Client();
            int client_id = int.Parse(comboBox1.SelectedItem.ToString());
            client = Ent.Clients.Find(client_id);
            if (client != null)
            {

                client.client_id = int.Parse(comboBox1.Text);
                client.client_name = textBox2.Text;
                client.client_phone = textBox3.Text;
                client.client_fax = textBox4.Text;
                client.client_mail = textBox5.Text;
                client.client_website = textBox6.Text;
                client.client_mobile = textBox7.Text;
                Ent.SaveChanges();
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text =
               textBox5.Text = textBox6.Text = textBox7.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Client is found and change id");
                textBox1.Text = string.Empty;
            }
            foreach (var d in Ent.Clients)
            {
                comboBox1.Items.Add((d.client_id).ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //delete client
            Client client = new Client();
            int client_id = int.Parse(comboBox1.SelectedItem.ToString());
            client = Ent.Clients.Find(client_id);
            if (client != null)
            {

                Ent.Clients.Remove(client);
                Ent.SaveChanges();
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text =
               textBox5.Text = textBox6.Text = textBox7.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Client is found and change id");
                textBox1.Text = string.Empty;
            }

            textBox1.Enabled = true;
            //update the client_id in combbox1
            foreach (var d in Ent.Clients)
            {
                comboBox1.Items.Add((d.client_id).ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Client client = new Client();
            int client_id = int.Parse(comboBox1.SelectedItem.ToString());
            client = Ent.Clients.Find(client_id);
            textBox2.Text = client.client_name;
            textBox1.Text = client.client_id.ToString();
            textBox4.Text = client.client_fax;
            textBox5.Text = client.client_mail;
            textBox6.Text = client.client_website;
            textBox7.Text = client.client_mobile;
        }
    }
}
