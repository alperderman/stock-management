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
    public partial class Form10 : Form
    {
        public Form2 PrevForm;

        public Form10(Form2 Form)
        {
            InitializeComponent();
            this.PrevForm = Form;
            Form.Hide();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            Form1.veriler.loadRapor();
            label4.Text = Form1.veriler.rapor["satistoplam"].ToString() + " TL";
            label5.Text = "-" + Form1.veriler.rapor["alistoplam"].ToString() + " TL";
            label6.Text = Form1.veriler.rapor["gelir"].ToString() + " TL";
        }

        private void Form10_FormClosed(object sender, FormClosedEventArgs e)
        {
            PrevForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
