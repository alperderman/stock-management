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
    public partial class Form8 : Form
    {
        public Form2 PrevForm;

        public Form8(Form2 Form)
        {
            InitializeComponent();
            this.PrevForm = Form;
            Form.Hide();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            Form1.veriler.loadUrunler();
            Form1.veriler.loadAlislar();
            refreshDataGrid();

            if (!Form1.veriler.checkIzin(new String[] { "a" }))
            {
                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;
                tableLayoutPanel1.ColumnStyles[0].Width = 0;
                tableLayoutPanel1.ColumnStyles[1].SizeType = SizeType.Absolute;
                tableLayoutPanel1.ColumnStyles[1].Width = 0;
            }
        }

        public void changeColumns()
        {
            dataGridView1.Columns[0].HeaderCell.Value = "ID";
            dataGridView1.Columns[1].HeaderCell.Value = "Ürün İsmi";
            dataGridView1.Columns[2].HeaderCell.Value = "Alış Fiyatı";
            dataGridView1.Columns[3].HeaderCell.Value = "Satış Fiyatı";
            dataGridView1.Columns[4].HeaderCell.Value = "Adet";

            dataGridView2.Columns[0].HeaderCell.Value = "Ürün İsmi";
            dataGridView2.Columns[1].HeaderCell.Value = "Alınan Adet";
            dataGridView2.Columns[2].HeaderCell.Value = "Fiyat";
            dataGridView2.Columns[3].HeaderCell.Value = "Alan";
        }

        public void refreshDataGrid()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView2.DataSource = null;
            this.dataGridView1.DataSource = Form1.veriler.urunler;
            this.dataGridView2.DataSource = Form1.veriler.alislar;
            dataGridView1.Update();
            dataGridView2.Update();
            dataGridView1.Refresh();
            dataGridView2.Refresh();
            changeColumns();
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        {
            PrevForm.Show();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                tableLayoutPanel2.Visible = true;
                string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string ad = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string afiyat = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

                label2.Text = id;
                label4.Text = ad;
                label6.Text = afiyat;
                label9.Text = "";

                label2.Visible = true;
                label4.Visible = true;
                label6.Visible = true;
                textBox1.Text = "";
                textBox1.Enabled = true;
            }
            else
            {
                label2.Visible = false;
                label4.Visible = false;
                label6.Visible = false;
                label9.Text = "";
                textBox1.Text = "";
                textBox1.Enabled = false;
                button1.Enabled = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                double afiyat = Convert.ToDouble(dataGridView1.SelectedRows[0].Cells[2].Value);
                int adet;
                double toplam;
                bool result = Int32.TryParse(textBox1.Text, out adet);
                if (textBox1.Text != "" && result)
                {
                    if (adet < 1)
                    {
                        textBox1.Text = "1";
                        toplam = afiyat;
                    }
                    else
                    {
                        toplam = afiyat * adet;
                    }
                    label9.Text = Convert.ToString(toplam);
                    button1.Enabled = true;
                }
                else
                {
                    label9.Text = "";
                    button1.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            string ad = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string adet = textBox1.Text;
            string fiyat = label9.Text;
            if (fiyat != "")
            {
                DialogResult dialogResult = MessageBox.Show(ad + " adlı üründen " + adet + " adet satın almak istediğinize emin misiniz?", "Eylem Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    string response = Form1.veriler.urunAl(id, Convert.ToInt32(adet), Convert.ToDouble(fiyat), Convert.ToString(Form1.veriler.kullanici["k_ad"]));
                    Form1.veriler.loadUrunler();
                    Form1.veriler.loadAlislar();
                    refreshDataGrid();
                    if (response == "")
                    {
                        MessageBox.Show("Ürün başarıyla satın alındı!", "Başarılı Eylem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(response, "Başarısız Eylem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else {
                MessageBox.Show("Lütfen adet giriniz!", "Başarısız Eylem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
