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
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }
         
        public void LoadData()
        {
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MiniStock;Integrated Security=True");
            //reading data from db and displaying them in DGV
            SqlDataAdapter sda = new SqlDataAdapter("select * from [MiniStock].[dbo].[Products]", con);//with SqlDataAdapter, no need to open and close the connection, it will do it automatically
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridViewProducts.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridViewProducts.Rows.Add();//add a row into DGV
                dataGridViewProducts.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridViewProducts.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])//(bool) item["ProductStatus"] helps to convert it into bool value
                {
                    dataGridViewProducts.Rows[n].Cells[2].Value = "ON";
                }
                else
                {
                    dataGridViewProducts.Rows[n].Cells[2].Value = "OFF";
                }

            }
        }
        private void Products_Load(object sender, EventArgs e)//everything inside will execute everytime this page loads
        {
            comboBoxStatus.SelectedIndex = 0;//it selects and show the 1st item inside the combobox
            LoadData();

        }
        public void Clear()
        {
            comboBoxStatus.SelectedIndex = 0;//it selects and show the 1st item inside the combobox
            textBoxProCode.Clear();
            textBoxProName.Clear();
        }
        private bool IfProductsExists(SqlConnection con, string productCode) //fucntion to check if prodruct exists into DB and we return the answer
        {
            SqlDataAdapter sda = new SqlDataAdapter("select 1 from [Products]  WHERE [ProductCode] = '" +productCode+ "'", con);//with SqlDataAdapter, no need to open and close the connection, it will do it automatically
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if(dt.Rows.Count >0)
                return true;
            else
                return false;
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //insert data into db
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MiniStock;Integrated Security=True");
            con.Open();
            bool status = false;
            if (comboBoxStatus.SelectedIndex == 0) //means if status is ON
            {
                status = true;
            }
            else
            {
                status = false;
            }
            var SqlQuery = "";
        

            //before inserting we check if the products already exists in DB
            if (IfProductsExists(con, textBoxProCode.Text))
            {
                //products exists, so we update the value's product
                SqlQuery = @"UPDATE [Products] SET [ProductName] = '" + textBoxProName.Text + "' ,[ProductStatus] = '" + status + "' WHERE [ProductCode] = '" + textBoxProCode.Text + "'";
                
            }
            else
            {
                //products does exists yet , so we insert it into db, we create it
                SqlQuery = @"INSERT INTO [dbo].[Products]
                 ([ProductCode]
                ,[ProductName]
                ,[ProductStatus])
                 VALUES
                ('" + textBoxProCode.Text + "','" + textBoxProName.Text + "','" + status + "')";

                Clear();
            }
            SqlCommand cmd = new SqlCommand(SqlQuery, con);

            cmd.ExecuteNonQuery();

            con.Close();

            //reading data from db and displaying them in DGV
            LoadData();
        }

        private void dataGridViewProducts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBoxProCode.Text = dataGridViewProducts.SelectedRows[0].Cells[0].Value.ToString();
            textBoxProName.Text = dataGridViewProducts.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridViewProducts.SelectedRows[0].Cells[2].Value.ToString()=="ON") 
            {
                comboBoxStatus.SelectedIndex = 0;
            }
            else
            {
                comboBoxStatus.SelectedIndex = 1;
            }

           
        }

        private void buttonDelete_Click(object sender, EventArgs e) //Delete the records
        {
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=MiniStock;Integrated Security=True");
            
            var SqlQuery = "";


            //before updating we check if the products exists in DB or not
            if (IfProductsExists(con, textBoxProCode.Text))
            {
                //products exists, so we delete the product
                con.Open();
                SqlQuery = @"DELETE FROM [Products]  WHERE [ProductCode] = '" + textBoxProCode.Text + "'";
                SqlCommand cmd = new SqlCommand(SqlQuery, con);

                cmd.ExecuteNonQuery();

                con.Close();
                Clear();
            }
            else
            {
                //products does not exists , so we can't update
                MessageBox.Show("Impossible \n THe product does not exists");
            }
            
            //reading data from db and displaying them in DGV
            LoadData();
        }

        private void buttonClearFields_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
