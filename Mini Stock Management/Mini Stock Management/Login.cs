using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mini_Stock_Management
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public void ClearFields()
        {
            textBoxPwd.Clear();
            textBoxUserName.Clear();
        }
        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearFields();
            textBoxUserName.Focus();//focus cursor should be on Username textbox field
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            //check login credentials
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MiniStock;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            
            SqlDataAdapter sda = new SqlDataAdapter(@"SELECT * FROM[dbo].[Login] where UserName = '"+textBoxUserName.Text+"' and Password = '"+textBoxPwd.Text+"'",con);
            //using SqlDataAdapter we don't need to open and close the connection. it will do it automatically
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 1)//if login correct
            {
                this.Hide();
                StockMain main = new StockMain(); //instantiatation
                main.Show();//call the StockMain page
            }
            else
            {
                MessageBox.Show("Invalid Username or password","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1_Click(sender, e);//we call this method 
            }
            

        }
    }
}
