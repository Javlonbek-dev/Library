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
    public partial class KitobOlish : Form
    {
        SqlConnection con;
        SqlCommand cmd;

        public KitobOlish()
        {
            InitializeComponent();
        }
        string queryAll = "SELECT QarzKitob.OKId, Kitoblar.Sarlavha, Azolar.Ismi, QarzKitob.olingan_sana,QarzKitob.qaytarish_sana FROM QarzKitob "+
             " INNER JOIN Kitoblar ON QarzKitob.KitobId= Kitoblar.KitobId" +
            " INNER JOIN Azolar ON QarzKitob.AzoId = Azolar.AzoId";

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

        private void KitobOlish_Load(object sender, EventArgs e)
        {
            dbConnection();
            showAllData(queryAll);
            loadKitob();
            loadAzo();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                dbConnection();
                int newOKId = int.Parse(textBox1.Text);
                int newKitobId = int.Parse(comboBox1.SelectedValue.ToString());
                int newAzoId = int.Parse(comboBox2.SelectedValue.ToString());
                string newolinganvaqt = dateTimePicker1.Value.Date.ToString("dd-MM-yyyy");
                string newqaytarishvaqt = dateTimePicker2.Value.Date.ToString("dd-MM-yyyy");
                cmd = new SqlCommand("insert into QarzKitob values (@OKId, @KitobId, @AzoId,@olingan_sanasi,@qaytarish_sanasi)", con);
                cmd.Parameters.AddWithValue("@OKId", newOKId);
                cmd.Parameters.AddWithValue("@KitobId", newKitobId);
                cmd.Parameters.AddWithValue("@AzoId", newAzoId);
                cmd.Parameters.AddWithValue("@olingan_sanasi", newolinganvaqt);
                cmd.Parameters.AddWithValue("@qaytarish_sanasi", newqaytarishvaqt);

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
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            dateTimePicker1.CalendarFont = null;
            dateTimePicker2.CalendarFont = null;
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
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<DateTime>(4);
                dtRow++;
            }
            con.Close();
        }
        private void loadKitob()
        {
            string sqlCommand = "Select * from Kitoblar";
            cmd = new SqlCommand(sqlCommand, con);
            SqlDataAdapter data = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            data.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Sarlavha";
            comboBox1.ValueMember = "KitobId";
            con.Close();
        }
        private void loadAzo()
        {
            string sqlCommand = "Select * from Azolar";
            cmd = new SqlCommand(sqlCommand, con);
            SqlDataAdapter data = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            data.Fill(dt);
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Ismi";
            comboBox2.ValueMember = "AzoId";
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbConnection();
            string query = "Update QarzKitob set OKId=@OKId, KitobId=@KitobId,AzoId=@AzoId,olingan_sana= @olingan_sanasi, qaytarish_sana=@qaytarish_sanasi where OKId=@OKId";
            cmd = new SqlCommand(query, con);
            int newOKId = int.Parse(textBox1.Text);
            int newKitobId = int.Parse(comboBox1.SelectedValue.ToString());
            int newAzoId = int.Parse(comboBox2.SelectedValue.ToString());
            string newolinganvaqt = dateTimePicker1.Value.Date.ToString("dd-MM-yyyy");
            string newqaytarishvaqt = dateTimePicker2.Value.Date.ToString("dd-MM-yyyy");
            cmd.Parameters.AddWithValue("@OKId", newOKId);
            cmd.Parameters.AddWithValue("@KitobId", newKitobId);
            cmd.Parameters.AddWithValue("@AzoId", newAzoId);
            cmd.Parameters.AddWithValue("@olingan_sanasi", newolinganvaqt);
            cmd.Parameters.AddWithValue("@qaytarish_sanasi", newqaytarishvaqt);
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
            string kitob = dataGridView1.Rows[row].Cells[1].Value.ToString();
            comboBox1.SelectedIndex = comboBox1.FindStringExact(kitob);
            string azo = dataGridView1.Rows[row].Cells[2].Value.ToString();
            comboBox2.SelectedIndex = comboBox2.FindStringExact(azo);
            if (dataGridView1.Rows[row].Cells[3].Value is DateTime borrowDate)
            {
                dateTimePicker1.Value = borrowDate;
            }

            if (dataGridView1.Rows[row].Cells[4].Value is DateTime returnDate)
            {
                dateTimePicker2.Value = returnDate;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dbConnection();
            int Id = int.Parse(textBox1.Text);
            string query = "Delete QarzKitob where OKId =@Id";
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
