using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_TelefonRehberi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TelefonRehberiDBEntities db = new TelefonRehberiDBEntities();
        private void Form1_Load(object sender, EventArgs e)
        {
            #region Sql veritabanı oluşturma
            /*
                 create database TelefonRehberiDB;
                    go
                    use TelefonRehberiDB;
                create table Kisiler(
                    ID int primary key identity(1,1) not null,
                    Ad nvarchar(25) not null,
                    Soyad nvarchar(25) not null,
                    Telefon nvarchar(10)
                                    ) 
                 */
            #endregion

            KisiListesi();
            btnGuncelle.Visible = false;
           
        }

        
        //metot listviewe ekleme
        private void KisiListesi()
        {
            listView1.Items.Clear();
            foreach (Kisiler item in db.Kisilers.ToList())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = item.ID.ToString();
                lvi.SubItems.Add(item.Ad);
                lvi.SubItems.Add(item.Soyad);
                lvi.SubItems.Add(item.Telefon);
                lvi.Tag = item;//***********************************önemli********************************
                listView1.Items.Add(lvi);
            }
        }
        //*************metot:ARAMA metodu
        private void KisiListesi(string param)
        {

            listView1.Items.Clear();
            //eğer parametredeki değer veritabanında bulunan ad isimli kolon içerisindeki karakter ile başlıyorsa o karakter ile eşleşen kisiyi aktar.
            var person = db.Kisilers.Where(x => x.Ad.StartsWith(param) || x.Soyad.StartsWith(param)).ToList();
            
            foreach (var item in person)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = item.ID.ToString();
                lvi.SubItems.Add(item.Ad);
                lvi.SubItems.Add(item.Soyad);
                lvi.SubItems.Add(item.Telefon);
                listView1.Items.Add(lvi);
                lvi.Tag = item;
                
            }
        }
        
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            //Kisiler kisi = new Kisiler();
            //kisi.Ad = txtAd.Text;
            //kisi.Soyad = txtSoyad.Text;
            //kisi.Telefon = txtTelefon.Text;

            //db.Kisilers.Add(kisi);
            //db.SaveChanges();

            db.Kisilers.Add(new Kisiler
            {
                
                Ad = txtAd.Text,
                Soyad = txtSoyad.Text,
                Telefon = txtTelefon.Text
            });

            bool sonuc = db.SaveChanges() > 0;

            MessageBox.Show(sonuc ? "Kişi Rehbere Eklendi" : "Ekleme Başarısız");

            KisiListesi();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)//arama textboxı
        {
            //arama işlemini gerçekleştiren metot.
            KisiListesi(txtAra.Text);
        }
        Kisiler guncellenecek;
        private void ListView1_DoubleClick(object sender, EventArgs e)//***************************
        {
            btnGuncelle.Visible = true;

            guncellenecek = listView1.SelectedItems[0].Tag as Kisiler;
            txtAd.Text = guncellenecek.Ad;
            txtSoyad.Text = guncellenecek.Soyad;
            txtTelefon.Text = guncellenecek.Telefon;



        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            guncellenecek.Ad = txtAd.Text;
            guncellenecek.Soyad = txtSoyad.Text;
            guncellenecek.Telefon = txtTelefon.Text;
            db.SaveChanges();
            btnGuncelle.Visible = false;
            KisiListesi();
        }
       // -----
        private void BtnSil_Click(object sender, EventArgs e)
        {
            int id = ((Kisiler)listView1.SelectedItems[0].Tag).ID;
            //(listView1.SelectedItems[0].Tag as Kisiler).
            Kisiler silinecek = db.Kisilers.Find(id);
            db.Kisilers.Remove(silinecek);
            db.SaveChanges();
            KisiListesi();
            
        }
    }
}
