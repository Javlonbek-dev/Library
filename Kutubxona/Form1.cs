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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Kutubxona
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;

        public Form1()
        {
            InitializeComponent();
        }
        string queryAll = "SELECT Kitoblar.KitobId, Kitoblar.Sarlavha, Kitoblar.Muallif, Kitoblar.Nashr_yili, Toifa.toifa_ismi FROM Kitoblar "+
            "INNER JOIN Toifa On Kitoblar.toifa_id=Toifa.toifa_id";

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
                int newkitob = int.Parse(textBox1.Text);
                string newsarlavha = textBox2.Text;
                string newmuallif = textBox3.Text;
                string newnashr = textBox4.Text;
                int newToifaId = int.Parse(comboBox1.SelectedValue.ToString());


                cmd = new SqlCommand("insert into Kitoblar values (@KtobId, @Sarlavha, @Maullif,@NashrYili,@toifa_id)", con);
                cmd.Parameters.AddWithValue("@KtobId", newkitob);
                cmd.Parameters.AddWithValue("@Sarlavha", newsarlavha);
                cmd.Parameters.AddWithValue("@Maullif", newmuallif);
                cmd.Parameters.AddWithValue("@NashrYili", newnashr);
                cmd.Parameters.AddWithValue("@toifa_id", newToifaId);
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
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<Int32>(3);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<string>(4);
                dtRow++;
            }
            con.Close();
        }

        private void cleardata()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedItem= null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dbConnection();
            showAllData(queryAll);
            loadToifa();
        }

        private void loadToifa()
        {
            string sqlCommand = "Select * from Toifa";
            cmd = new SqlCommand(sqlCommand, con);
            SqlDataAdapter data = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            data.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "toifa_ismi";
            comboBox1.ValueMember = "toifa_id";
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbConnection();
            string query = "Update Kitoblar set KitobId=@KtobId,Sarlavha=@Sarlavha,Muallif= @Maullif,Nashr_yili=@NashrYili,toifa_id=@toifa_id where KitobId=@KtobId";
            cmd = new SqlCommand(query, con);
            int newkitob = int.Parse(textBox1.Text);
            string newsarlavha = textBox2.Text;
            string newmuallif = textBox3.Text;
            string newnashr = textBox4.Text;
            int newToifaId = int.Parse(comboBox1.SelectedValue.ToString());


            cmd.Parameters.AddWithValue("@KtobId", newkitob);
            cmd.Parameters.AddWithValue("@Sarlavha", newsarlavha);
            cmd.Parameters.AddWithValue("@Maullif", newmuallif);
            cmd.Parameters.AddWithValue("@NashrYili", newnashr); 
            cmd.Parameters.AddWithValue("@toifa_id", newToifaId);

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
            textBox4.Text = dataGridView1.Rows[row].Cells[3].Value.ToString();
            string toifa = dataGridView1.Rows[row].Cells[4].Value.ToString();
            comboBox1.SelectedIndex = comboBox1.FindStringExact(toifa);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dbConnection();
            int Id = int.Parse(textBox1.Text);
            string query = "Delete Kitoblar where KitobId =@Id";
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
