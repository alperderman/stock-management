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
    public partial class Form6 : Form
    {
        public Form5 PrevForm;

        public Form6(Form5 Form)
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
            string k_ad = textBox1.Text.ToString();
            string k_sifre = textBox2.Text.ToString();
            string isim = textBox3.Text.ToString();
            string soyisim = textBox4.Text.ToString();
            string cinsiyet = comboBox1.GetItemText(comboBox1.SelectedItem);
            string unvan = comboBox2.GetItemText(comboBox2.SelectedItem);

            string response = Form1.veriler.calisanEkle(k_ad, k_sifre, isim, soyisim, cinsiyet, unvan);
            Form1.veriler.loadCalisanlar();
            PrevForm.refreshDataGrid();
            if(response == ""){
                MessageBox.Show("Çalışan başarıyla oluşturuldu!", "Başarılı Eylem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }else{
                MessageBox.Show(response, "Başarısız Eylem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            this.Close();
        }

        public void loadUnvanCombobox()
        {
            foreach (DataRow row in Form1.veriler.unvanlar.Rows)
            {
                if (Form1.veriler.checkIzin(new String[] { "cr" }))
                {
                    if (Convert.ToString(row["id"]) != "ptrn" && Convert.ToString(row["id"]) != "inka")
                    {
                        comboBox2.Items.Add(Convert.ToString(row["ad"]));
                    }
                }
                else {
                    comboBox2.Items.Add(Convert.ToString(row["ad"]));
                }
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            loadUnvanCombobox();
        }
    }
}
