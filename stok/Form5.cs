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
    public partial class Form5 : Form
    {
        public Form2 PrevForm;

        public Form5(Form2 Form)
        {
            InitializeComponent();
            this.PrevForm = Form;
            Form.Hide();
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            Form1.veriler.loadCalisanlar();
            refreshDataGrid();
            loadUnvanCombobox();
            
            if (!Form1.veriler.checkIzin(new String[] {"c", "cr"}))
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
                if (control.GetType() == typeof(TextBox))
                {
                    ((TextBox)control).Enabled = false;
                }
                if (control.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)control).Enabled = false;
                }
                if (control.GetType() == typeof(Button))
                {
                    if (((Button)control).Text != "İptal")
                    {
                        ((Button)control).Enabled = false;
                    }
                }

                if (control.Controls.Count > 0)
                {
                    lockControls(control);
                }

            }
        }

        public void changeColumns()
        {
            dataGridView1.Columns[0].HeaderCell.Value = "Kullanıcı Adı";
            dataGridView1.Columns[1].HeaderCell.Value = "İsim";
            dataGridView1.Columns[2].HeaderCell.Value = "Soyisim";
            dataGridView1.Columns[3].HeaderCell.Value = "Cinsiyet";
            dataGridView1.Columns[4].HeaderCell.Value = "Ünvan";
        }

        public void loadUnvanCombobox() {
            foreach (DataRow row in Form1.veriler.unvanlar.Rows)
            {
                comboBox2.Items.Add(Convert.ToString(row["ad"]));
            }
        }

        public void refreshDataGrid()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Form1.veriler.calisanlar;
            dataGridView1.Update();
            dataGridView1.Refresh();
            changeColumns();
            tableLayoutPanel2.Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.ClearSelection();
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms.OfType<Form6>().Any())
            {
                Application.OpenForms.OfType<Form6>().First().Close();
            }

            PrevForm.Show();
        }

        private bool checkSelf() {
            bool validate = false;
            string k_ad = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            if(k_ad == Convert.ToString(Form1.veriler.kullanici["k_ad"])){
                validate = true;
            }
            return validate;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                tableLayoutPanel2.Visible = true;
                string isim = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string soyisim = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string cinsiyet = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string unvan = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();

                textBox1.Text = isim;
                textBox2.Text = soyisim;

                if (cinsiyet == "E")
                {
                    cinsiyet = "Erkek";
                }else {
                    cinsiyet = "Kadın";
                }

                comboBox1.SelectedItem = cinsiyet;
                comboBox2.SelectedItem = unvan;

                if (checkSelf())
                {
                    button1.Enabled = false;
                }
                else
                {
                    button1.Enabled = true;
                }

                if (Form1.veriler.checkIzin(new String[] { "cr" }))
                {
                    string unvanid = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                    if (unvanid == "ptrn" || unvanid == "inka")
                    {
                        button1.Enabled = false;
                        button2.Enabled = false;
                    }
                    else {
                        button1.Enabled = true;
                        button2.Enabled = true;
                    }
                }
                else if (!Form1.veriler.checkIzin(new String[] { "c", "cr" }))
                {
                    button1.Enabled = false;
                    button2.Enabled = false;
                }
            }
            else
            {
                tableLayoutPanel2.Visible = false;
            }

        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Form6>().Any())
            {
                Application.OpenForms.OfType<Form6>().First().BringToFront();
            }
            else
            {
                Form6 Form6 = new Form6(this);
                Form6.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string k_ad = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string isim = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string soyisim = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string unvan = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            DialogResult dialogResult = MessageBox.Show(isim+" "+soyisim+" isimli çalışanı silmek istediğinize emin misiniz?", "Eylem Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                string response =Form1.veriler.calisanSil(k_ad, unvan);
                Form1.veriler.loadCalisanlar();
                refreshDataGrid();
                if (response == "")
                {
                    MessageBox.Show("Çalışan başarıyla silindi!", "Başarılı Eylem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(response, "Başarısız Eylem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string eskiisim = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string eskisoyisim = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string eskiunvan = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            string k_ad = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string isim = textBox1.Text.ToString();
            string soyisim = textBox2.Text.ToString();
            string cinsiyet = comboBox1.GetItemText(comboBox1.SelectedItem);
            string unvan = comboBox2.GetItemText(comboBox2.SelectedItem);

            DialogResult dialogResult = MessageBox.Show(eskiisim+" "+eskisoyisim+" isimli çalışanı güncellemek istediğinize emin misiniz?", "Eylem Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                string response = Form1.veriler.calisanGuncelle(k_ad, isim, soyisim, cinsiyet, unvan, eskiunvan);
                Form1.veriler.loadCalisanlar();
                refreshDataGrid();
                if (response == "")
                {
                    MessageBox.Show("Çalışan başarıyla güncellendi!", "Başarılı Eylem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(response, "Başarısız Eylem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
