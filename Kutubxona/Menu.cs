using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kutubxona
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Azolar form1 = new Azolar();
            form1.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1= new Form1();
            form1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Toifacs form1 = new Toifacs();
            form1.Show(); this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            KitobOlish kitobOlish = new KitobOlish();
            kitobOlish.Show(); this.Hide();
        }
    }
}
