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
    public partial class Form3 : Form
    {
        public Form2 PrevForm;

        public Form3(Form2 Form)
        {
            InitializeComponent();
            this.PrevForm = Form;
            Form.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Form1.veriler.loadUrunler();
            refreshDataGrid();

            if (!Form1.veriler.checkIzin(new String[] {"u"}))
            {
                toolStripMenuItem2.Enabled = false;
                lockControls(this);
                tableLayoutPanel1.ColumnStyles[1].SizeType = SizeType.Absolute;
                tableLayoutPanel1.ColumnStyles[1].Width = 0;
            }
        }

        public void lockControls(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                if (control.GetType() == typeof(TextBox)) { 
                    ((TextBox)control).Enabled = false;
                }
                if (control.GetType() == typeof(ComboBox)) {
                    ((ComboBox)control).Enabled = false;
                }
                if (control.GetType() == typeof(Button))
                {
                    if (((Button)control).Text != "İptal")
                    {
                        ((Button)control).Enabled = false;
                    }
                }

                if (control.Controls.Count > 0) {
                    lockControls(control);
                }
                    
            }
        }

        public void changeColumns()
        {
            dataGridView1.Columns[0].HeaderCell.Value = "ID";
            dataGridView1.Columns[1].HeaderCell.Value = "Ürün İsmi";
            dataGridView1.Columns[2].HeaderCell.Value = "Alış Fiyatı";
            dataGridView1.Columns[3].HeaderCell.Value = "Satış Fiyatı";
            dataGridView1.Columns[4].HeaderCell.Value = "Adet";
        }

        public void refreshDataGrid()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Form1.veriler.urunler;
            dataGridView1.Update();
            dataGridView1.Refresh();
            changeColumns();
            tableLayoutPanel2.Visible = false;
            dataGridView1.ClearSelection();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms.OfType<Form4>().Any())
            {
                Application.OpenForms.OfType<Form4>().First().Close();
            }

            PrevForm.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                tableLayoutPanel2.Visible = true;
                string ad = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string afiyat = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string sfiyat = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string adet = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();

                textBox1.Text = ad;
                textBox2.Text = afiyat;
                textBox3.Text = sfiyat;
            }
            else {
                tableLayoutPanel2.Visible = false;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Form4>().Any())
            {
                Application.OpenForms.OfType<Form4>().First().BringToFront();
            }
            else
            {
                Form4 Form4 = new Form4(this);
                Form4.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string ad = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            DialogResult dialogResult = MessageBox.Show(ad + " adlı ürünü silmek istediğinize emin misiniz?", "Eylem Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                string response = Form1.veriler.urunSil(id);
                Form1.veriler.loadUrunler();
                refreshDataGrid();
                if (response == "")
                {
                    MessageBox.Show("Ürün başarıyla silindi!", "Başarılı Eylem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(response, "Başarısız Eylem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string eskiad = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string ad = textBox1.Text.ToString();
            double alisfiyat = Convert.ToDouble(textBox2.Text);
            double satisfiyat = Convert.ToDouble(textBox3.Text);
            DialogResult dialogResult = MessageBox.Show(eskiad + " adlı ürünü güncellemek istediğinize emin misiniz?", "Eylem Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                string response = Form1.veriler.urunGuncelle(id, ad, alisfiyat, satisfiyat);
                Form1.veriler.loadUrunler();
                refreshDataGrid();
                if (response == "")
                {
                    MessageBox.Show("Ürün başarıyla güncellendi!", "Başarılı Eylem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(response, "Başarısız Eylem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
