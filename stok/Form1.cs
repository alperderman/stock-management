using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;

namespace stok2
{

    public partial class Form1 : Form
    {
        public static Veriler veriler = new Veriler();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            veriler.loadKullanicilar();
            veriler.loadUnvanlar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            login();
        }

        public void login()
        {
            bool validate = false;
            foreach (DataRow calisan in veriler.kullanicilar.Rows)
            {
                string ad = textBox1.Text;
                string sifre = textBox2.Text;
                if (ad == calisan["k_ad"].ToString() && sifre == calisan["k_sifre"].ToString())
                {
                    veriler.kullanici = calisan;
                    validate = true;
                    break;
                }
            }

            if (validate)
            {
                string suffix;
                if (veriler.kullanici["cinsiyet"].ToString() == "E")
                {
                    suffix = "bey";
                }
                else
                {
                    suffix = "hanım";
                }
                MessageBox.Show("Başarıyla giriş yapıldı! Holgeldiniz " + veriler.kullanici["isim"].ToString() + " " + suffix, "Başarılı Eylem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form2 Form2 = new Form2(this);
                Form2.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!", "Başarısız Eylem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox1.Focus();
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                login();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            veriler.loadKullanicilar();
            veriler.loadUnvanlar();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox1.Focus();
        }
    }

    public class Veriler
    {
        public OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0; Data Source=../../../stok.accdb");
        public OleDbDataAdapter adaptor;
        public OleDbCommand komut;
        public DataSet stok;
        public DataTable calisanlar;
        public DataTable urunler;
        public DataTable unvanlar;
        public DataTable alislar;
        public DataTable satislar;
        public DataTable kullanicilar;
        public DataRow rapor;
        public DataRow kullanici;

        public void loadKullanicilar()
        {
            adaptor = new OleDbDataAdapter("select c.k_ad, c.k_sifre, c.isim, c.soyisim, c.cinsiyet, u.ad as unvan, u.izin as izinler from calisanlar as c,unvanlar as u where c.unvan_id = u.id", baglanti);
            stok = new DataSet();
            kullanicilar = new DataTable();
            baglanti.Open();
            adaptor.Fill(stok, "calisanlar");
            kullanicilar = stok.Tables["calisanlar"];
            baglanti.Close();
        }

        public void loadRapor()
        {
            adaptor = new OleDbDataAdapter("SELECT sum(a.fiyat) as alistoplam, sum(s.fiyat) as satistoplam, sum(s.fiyat)-sum(a.fiyat) as gelir from alislar as a, satislar as s", baglanti);
            stok = new DataSet();
            alislar = new DataTable();
            baglanti.Open();
            adaptor.Fill(stok, "rapor");
            rapor = stok.Tables["rapor"].Rows[0];
            baglanti.Close();
        }

        public void loadAlislar()
        {
            adaptor = new OleDbDataAdapter("select u.ad as urunad, a.adet, a.fiyat, c.isim & ' ' & c.soyisim as alan from alislar as a,urunler as u,calisanlar as c where a.u_id = u.id and a.alan = c.k_ad order by a.id desc", baglanti);
            stok = new DataSet();
            alislar = new DataTable();
            baglanti.Open();
            adaptor.Fill(stok, "alislar");
            alislar = stok.Tables["alislar"];
            baglanti.Close();
        }

        public void loadSatislar()
        {
            adaptor = new OleDbDataAdapter("select u.ad as urunad, s.adet, s.fiyat, c.isim & ' ' & c.soyisim as satan from satislar as s,urunler as u,calisanlar as c where s.u_id = u.id and s.satan = c.k_ad order by s.id desc", baglanti);
            stok = new DataSet();
            satislar = new DataTable();
            baglanti.Open();
            adaptor.Fill(stok, "satislar");
            satislar = stok.Tables["satislar"];
            baglanti.Close();
        }

        public string urunAl(Int32 u_id, Int32 adet, double fiyat, string alan) {
            string response = "";
            if (Form1.veriler.checkIzin(new String[] { "a" }))
            {
                komut = new OleDbCommand();
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "insert into alislar (u_id, adet, fiyat, alan) values (" + u_id + ", " + adet + ", " + fiyat + ", '" + alan + "')";
                komut.ExecuteNonQuery();
                komut.CommandText = "update urunler set adet = adet + " + adet + " where id = " + u_id;
                komut.ExecuteNonQuery();
                baglanti.Close();
                
            }
            else {
                response = "Ürün satın alma izininiz yok!";
            }
            return response;
        }

        public string urunSat(Int32 u_id, Int32 adet, double fiyat, string satan)
        {
            string response = "";
            if (Form1.veriler.checkIzin(new String[] { "s" }))
            {
                komut = new OleDbCommand();
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "insert into satislar (u_id, adet, fiyat, satan) values (" + u_id + ", " + adet + ", " + fiyat + ", '" + satan + "')";
                komut.ExecuteNonQuery();
                komut.CommandText = "update urunler set adet = adet - " + adet + " where id = " + u_id;
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else
            {
                response = "Ürün satma izininiz yok!";
            }
            return response;
        }

        public void loadUnvanlar()
        {
            adaptor = new OleDbDataAdapter("select * from unvanlar", baglanti);
            stok = new DataSet();
            unvanlar = new DataTable();
            baglanti.Open();
            adaptor.Fill(stok, "unvanlar");
            unvanlar = stok.Tables["unvanlar"];
            baglanti.Close();
        }

        public bool checkIzin(string[] izinler) {
            bool validate = false;
            string[] k_izinler = Convert.ToString(kullanici["izinler"]).Split('-');

            foreach(string k_izin in k_izinler){
                if (validate){
                    break;
                }

                foreach(string izin in izinler){
                    if(izin == k_izin){
                        validate = true;
                        break;
                    }
                }
            }

            return validate;
        }

        public void sifreDegistir(string eskisifre, string yenisifre) {
            komut = new OleDbCommand();
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "update calisanlar set k_sifre = '" + yenisifre + "' where k_ad = '" + kullanici["k_ad"] + "'";
            komut.ExecuteNonQuery();
            baglanti.Close();
        }

        public string getUnvanid(string unvan)
        {
            string unvanid = "";

            foreach(DataRow row in unvanlar.Rows)
            {
                if(Convert.ToString(row["ad"]) == unvan){
                    unvanid = Convert.ToString(row["id"]);
                    break;
                }
            }

            return unvanid;
        }

        public void loadCalisanlar()
        {
            adaptor = new OleDbDataAdapter("select c.k_ad, c.isim, c.soyisim, c.cinsiyet, u.ad as unvan, u.id as u_id from calisanlar as c,unvanlar as u where c.unvan_id = u.id", baglanti);
            stok = new DataSet();
            calisanlar = new DataTable();
            baglanti.Open();
            adaptor.Fill(stok, "calisanlar");
            calisanlar = stok.Tables["calisanlar"];
            baglanti.Close();
        }

        public string calisanEkle(string k_ad, string k_sifre, string isim, string soyisim, string cinsiyet, string unvan)
        {
            string response = "";
            if (Form1.veriler.checkIzin(new String[] {"c", "cr"}))
            {
                string unvanid = Form1.veriler.getUnvanid(unvan);
                bool validate = true;

                foreach (DataRow calisan in kullanicilar.Rows)
                {
                    if (k_ad == calisan["k_ad"].ToString())
                    {
                        validate = false;
                        response = "Bu kullanıcı adında başka bir çalışan mevcut! Lütfen başka bir kullanıcı adı yazınız.";
                        break;
                    }
                }

                if (Form1.veriler.checkIzin(new String[] {"cr"}))
                {
                    if (unvanid == "ptrn" || unvanid == "inka")
                    {
                        validate = false;
                        response = "Patron yada insan kaynakları ünvanına sahip bir çalışan eklemenize izininiz yok!";
                    }
                }

                if(validate){
                    if (cinsiyet == "Erkek")
                    {
                        cinsiyet = "E";
                    }
                    else
                    {
                        cinsiyet = "K";
                    }

                    komut = new OleDbCommand();
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "insert into calisanlar (k_ad, k_sifre, isim, soyisim, cinsiyet, unvan_id) values ('" + k_ad + "', '" + k_sifre + "', '" + isim + "', '" + soyisim + "', '" + cinsiyet + "', '" + unvanid + "')";
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
                
            }
            else {
                response = "Çalışan ekleme izininiz yok!";
            }

            return response;
        }

        public string calisanSil(string k_ad, string unvan)
        {
            string response = "";

            if (Form1.veriler.checkIzin(new String[] {"c", "cr"}))
            {
                string unvanid = Form1.veriler.getUnvanid(unvan);
                bool validate = true;

                if (Form1.veriler.checkIzin(new String[] {"cr"}))
                {
                    if (unvanid == "ptrn" || unvanid == "inka")
                    {
                        validate = false;
                        response = "Patron yada insan kaynakları ünvanına sahip bir çalışanı silme izininiz yok!";
                    }
                }

                if(k_ad == Convert.ToString(kullanici["k_ad"])){
                    validate = false;
                    response = "Kendinizi silemezsiniz!";
                }

                if(validate){
                    komut = new OleDbCommand();
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "delete from calisanlar where k_ad = @k_ad";
                    komut.Parameters.AddWithValue("@k_ad", k_ad);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
                
            }
            else
            {
                response = "Çalışan silme izininiz yok!";
            }

            return response;
        }

        public string calisanGuncelle(string k_ad, string isim, string soyisim, string cinsiyet, string unvan, string eskiunvan)
        {
            string response = "";

            if (Form1.veriler.checkIzin(new String[] {"c", "cr"}))
            {
                string unvanid = Form1.veriler.getUnvanid(unvan);
                string eskiunvanid = Form1.veriler.getUnvanid(eskiunvan);
                bool validate = true;

                if (Form1.veriler.checkIzin(new String[] {"cr"}))
                {
                    if (unvanid == "ptrn" || unvanid == "inka")
                    {
                        validate = false;
                        response = "Patron yada insan kaynakları ünvanına sahip bir çalışan olarak güncellemeye izininiz yok!";
                    }
                    else if (eskiunvanid == "ptrn" || eskiunvanid == "inka")
                    {
                        validate = false;
                        response = "Patron yada insan kaynakları ünvanına sahip bir çalışanı güncellemeye izininiz yok!";
                    }
                    else if (k_ad == Convert.ToString(kullanici["k_ad"]))
                    {
                        validate = false;
                        response = "Kendinizi güncelleyemezsiniz!";
                    }
                }

                if(validate){
                    if (cinsiyet == "Erkek")
                    {
                        cinsiyet = "E";
                    }
                    else
                    {
                        cinsiyet = "K";
                    }

                    komut = new OleDbCommand();
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "update calisanlar set isim = '" + isim + "', soyisim = '" + soyisim + "', cinsiyet = '" + cinsiyet + "', unvan_id = '" + unvanid + "' where k_ad = '" + k_ad + "'";
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
                
            }
            else
            {
                response = "Çalışan güncelleme izininiz yok!";
            }


            return response;
        }

        public void loadUrunler()
        {
            adaptor = new OleDbDataAdapter("select * from urunler", baglanti);
            stok = new DataSet();
            urunler = new DataTable();
            baglanti.Open();
            adaptor.Fill(stok, "urunler");
            urunler = stok.Tables["urunler"];
            baglanti.Close();
        }

        public string urunEkle(string ad, double alisfiyat, double satisfiyat){
            string response = "";

            if (Form1.veriler.checkIzin(new String[] {"u"}))
            {
                komut = new OleDbCommand();
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "insert into urunler (ad, alisfiyat, satisfiyat, adet) values ('" + ad + "', " + alisfiyat + ", " + satisfiyat + ", 0)";
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else {
                response = "Ürün ekleme izininiz yok!";
            }

            return response;
        }

        public string urunSil(string id)
        {
            string response = "";

            if (Form1.veriler.checkIzin(new String[] { "u" }))
            {
                komut = new OleDbCommand();
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "delete from urunler where id = @id";
                komut.Parameters.AddWithValue("@id", id);
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else {
                response = "Ürün silme izininiz yok!";
            }

            return response;
        }

        public string urunGuncelle(string id, string ad, double alisfiyat, double satisfiyat)
        {
            string response = "";

            if (Form1.veriler.checkIzin(new String[] { "u" }))
            {
                komut = new OleDbCommand();
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "update urunler set ad = '" + ad + "', alisfiyat = " + alisfiyat + ", satisfiyat = " + satisfiyat + " where id = " + id + "";
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else {
                response = "Ürün güncelleme izininiz yok!";
            }

            return response;
        }
    }
}
