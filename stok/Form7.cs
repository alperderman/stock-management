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
    public partial class Form7 : Form
    {
        public Form2 PrevForm;

        public Form7(Form2 Form)
        {
            InitializeComponent();
            this.PrevForm = Form;
            Form.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form7_FormClosed(object sender, FormClosedEventArgs e)
        {
            PrevForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string eskisifre = textBox1.Text.ToString();
            string yenisifre = textBox2.Text.ToString();

            if(eskisifre == Convert.ToString(Form1.veriler.kullanici["k_sifre"])){
                DialogResult dialogResult = MessageBox.Show("Şifrenizi değiştirmek istediğinize emin misiniz?", "Eylem Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    Form1.veriler.sifreDegistir(eskisifre, yenisifre);
                    Form1.veriler.kullanici["k_sifre"] = yenisifre;
                    MessageBox.Show("Şifreniz başarıyla değiştirildi!", "Başarılı Eylem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }else{
                MessageBox.Show("Girdiğiniz şifre hatalı!", "Başarısız Eylem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
