﻿using System;
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
    public partial class Toifacs : Form
    {
        SqlConnection con;
        SqlCommand cmd;

        public Toifacs()
        {
            InitializeComponent();
        }
        string queryAll = "SELECT Toifa.toifa_id, Toifa.toifa_ismi FROM Toifa ";

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
                int newtoifaId = int.Parse(textBox1.Text);
                string newtoifaismi = textBox2.Text;
                cmd = new SqlCommand("insert into Toifa values (@toifa_id, @toifa_ismi)", con);
                cmd.Parameters.AddWithValue("@toifa_id", newtoifaId);
                cmd.Parameters.AddWithValue("@toifa_ismi", newtoifaismi);

                cmd.ExecuteNonQuery();
                showAllData(queryAll);
                con.Open();
                con.Close();
                cleardata();
                MessageBox.Show("New record added successfully");

            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input. Please enter a valid number for null fields.");
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
                dtRow++;
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbConnection();
            string query = "Update Toifa set toifa_id=@toifa_id,toifa_ismi=@toifa_ismi where toifa_id=@toifa_id";
            cmd = new SqlCommand(query, con);
            int newtoifaId = int.Parse(textBox1.Text);
            string newtoifaismi = textBox2.Text;
            cmd.Parameters.AddWithValue("@toifa_id", newtoifaId);
            cmd.Parameters.AddWithValue("@toifa_ismi", newtoifaismi);
            cmd.ExecuteNonQuery();
            showAllData(queryAll);
            con.Open();
            con.Close();
            cleardata();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dbConnection();
            int Id = int.Parse(textBox1.Text);
            string query = "Delete Toifa where toifa_id =@Id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Ma'lumot o'chirildi!");
            cleardata();
            showAllData(queryAll);
        }

        private void Toifacs_Load(object sender, EventArgs e)
        {
            dbConnection();
            showAllData(queryAll);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            textBox1.Text = dataGridView1.Rows[row].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[row].Cells[1].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }
    }
}
