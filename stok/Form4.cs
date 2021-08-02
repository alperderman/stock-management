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
    public partial class Form4 : Form
    {
        public Form3 PrevForm;

        public Form4(Form3 Form)
        {
            InitializeComponent();
            this.PrevForm = Form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ad = textBox1.Text.ToString();
            double alisfiyat = Convert.ToDouble(textBox2.Text);
            double satisfiyat = Convert.ToDouble(textBox3.Text);

            string response = Form1.veriler.urunEkle(ad, alisfiyat, satisfiyat);
            Form1.veriler.loadUrunler();
            PrevForm.refreshDataGrid();
            if (response == "")
            {
                MessageBox.Show("Ürün başarıyla oluşturuldu!", "Başarılı Eylem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(response, "Başarısız Eylem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            this.Close();
        }

    }
}
