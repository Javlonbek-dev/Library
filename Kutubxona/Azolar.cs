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

namespace Kutubxona
{
    public partial class Azolar : Form
    {
        SqlConnection con;
        SqlCommand cmd;

        public Azolar()
        {
            InitializeComponent();
        }
        string queryAll = "SELECT Azolar.AzoId, Azolar.Ismi, Azolar.email, Azolar.azo_vaqti FROM Azolar ";

        private void dbConnection()
        {
            string strConnection = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Kutubxona;Integrated Security=True";

            try
            {
                con = new SqlConnection(strConnection);
                con.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dbConnection();
                int newAzoId = int.Parse(textBox1.Text);
                string newismi = textBox2.Text;
                string newemail = textBox3.Text;
                string newvaqt = dateTimePicker1.Value.Date.ToString("dd-MM-yyyy");
                cmd = new SqlCommand("insert into Azolar values (@AzoId, @Ismi, @email,@azo_vaqti)", con);
                cmd.Parameters.AddWithValue("@AzoId", newAzoId);
                cmd.Parameters.AddWithValue("@Ismi", newismi);
                cmd.Parameters.AddWithValue("@email", newemail);
                cmd.Parameters.AddWithValue("@azo_vaqti", newvaqt);

                cmd.ExecuteNonQuery();
                showAllData(queryAll);
                con.Open();
                con.Close();
                cleardata();
                MessageBox.Show("New record added successfully");

            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input. Please enter a valid number for Narxi and Miqdor fields.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the new record: " + ex.Message);
            }
        }
        private void cleardata()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            dateTimePicker1.CalendarFont = null;
        }
        private void showAllData(string sorov)
        {
            cmd = new SqlCommand(sorov, con);
            SqlDataAdapter data = new SqlDataAdapter();
            data.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dataGridView1.Rows.Clear();
            data.Fill(dt);
            int column, dtRow = 0;

            foreach (DataRow row in dt.Rows)
            {
                column = 0;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<Int32>(0);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<string>(1);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<string>(2);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<DateTime>(3);
                dtRow++;
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbConnection();
            string query = "Update Azolar set AzoId=@AzoId,Ismi=@Ismi,email= @email,azo_vaqti=@azo_vaqti where AzoId=@AzoId";
            cmd = new SqlCommand(query, con);
            int newAzoId = int.Parse(textBox1.Text);
            string newismi = textBox2.Text;
            string newemail = textBox3.Text;
            string newvaqt = dateTimePicker1.Value.Date.ToString("dd-MM-yyyy");
            cmd.Parameters.AddWithValue("@AzoId", newAzoId);
            cmd.Parameters.AddWithValue("@Ismi", newismi);
            cmd.Parameters.AddWithValue("@email", newemail);
            cmd.Parameters.AddWithValue("@azo_vaqti", newvaqt);
            cmd.ExecuteNonQuery();
            showAllData(queryAll);
            con.Open();
            con.Close();
            cleardata();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            textBox1.Text = dataGridView1.Rows[row].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[row].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[row].Cells[2].Value.ToString();
            dataGridView1.Rows[row].Cells[3].Value = dateTimePicker1.Value.ToShortDateString();
        }

        private void Azolar_Load(object sender, EventArgs e)
        {
            dbConnection();
            showAllData(queryAll);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dbConnection();
            int Id = int.Parse(textBox1.Text);
            string query = "Delete Azolar where AzoId =@Id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Ma'lumot o'chirildi!");
            cleardata();
            showAllData(queryAll);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }
    }
}
