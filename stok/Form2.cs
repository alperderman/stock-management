using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stok2
{
    public partial class Form2 : Form
    {
        public Form1 PrevForm;
        public Form2(Form1 Form)
        {
            InitializeComponent();
            this.PrevForm = Form;
            Form.Hide();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Form1.veriler.loadUrunler();
            int kritik = 0;
            foreach(DataRow urun in Form1.veriler.urunler.Rows){
                if(Convert.ToInt32(urun["adet"]) <= 20){
                    kritik++;
                }
            }
            if(kritik > 0){
                MessageBox.Show("Depoda, kritik sayıda "+kritik+" adet ürün var!", "Kritik Stok Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            PrevForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 Form3 = new Form3(this);
            Form3.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 Form5 = new Form5(this);
            Form5.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form7 Form7 = new Form7(this);
            Form7.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form8 Form8 = new Form8(this);
            Form8.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form9 Form9 = new Form9(this);
            Form9.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form10 Form10 = new Form10(this);
            Form10.Show();
        }

    }
}
