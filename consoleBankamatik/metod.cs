using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleBankamatik
{
    class metod
    {
        // yeni hesap ekleme metodu
        public static void HesapEkle(string ad, int sifre, int bakiye)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection("server=DESKTOP-L799HNF\\SQLEXPRESS; Initial Catalog=test; Integrated Security=True");
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "insert into hesap values (@ad, @bakiye, @sifre)";
            cmd.Parameters.AddWithValue("@ad", ad);
            cmd.Parameters.AddWithValue("@bakiye", bakiye);
            cmd.Parameters.AddWithValue("@sifre", sifre);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //işlemler arasına boşluk/ çizgi ekleme
        public static void AraEkle()
        {
            Console.WriteLine("");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("");
        }


        //girilen hesap no ve şifre eşleşmesini kontrol edilmesi
        public static bool HesapDorula(int hesapNo, int sifre)
        {
            SqlConnection con = new SqlConnection("server=DESKTOP-L799HNF\\SQLEXPRESS; Initial Catalog=test; Integrated Security=True");
            string sorgu = "select count(*) from hesap where hesapNo = @hesapNo and hesapSifre = @hesapSifre";
            SqlCommand cmd = new SqlCommand(sorgu, con);
            cmd.Parameters.AddWithValue("@hesapNo", hesapNo);
            cmd.Parameters.AddWithValue("@hesapSifre", sifre);
            con.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            if (a == 1) { return true; }
            else { return false; }

        }
        public static string HesapIsmi (int hesapNo)
        {
            SqlConnection con = new SqlConnection("server = DESKTOP-L799HNF\\SQLEXPRESS; Initial Catalog = test; Integrated Security = True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select hesapAd from hesap where hesapNo = @hesapNo";
            cmd.Parameters.AddWithValue("@hesapNo", hesapNo);
            con.Open();
            string ad = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return ad;
        }
        public static int Bakiye(int hesapNo)
        {
            
            SqlConnection con = new SqlConnection("server=DESKTOP-L799HNF\\SQLEXPRESS; Initial Catalog=test; Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select hesapBakiye from hesap where hesapNo = @hesapNo";
            cmd.Parameters.AddWithValue("@hesapNo", hesapNo);
            con.Open();
            int bakiye = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return bakiye;
        }
        public static void ParaCek (int hesapNo, int miktar)
        {
            // para çekilme işlemi
            SqlConnection con = new SqlConnection("server=DESKTOP-L799HNF\\SQLEXPRESS; Initial Catalog=test; Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "update hesap set hesapBakiye = hesapBakiye - @miktar where hesapNo = @hesapNo";
            cmd.Parameters.AddWithValue("@hesapNo", hesapNo);
            cmd.Parameters.AddWithValue("@miktar", miktar);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            // hesap özetine ekleme
            cmd.CommandText = "insert into islem values(@date, @islemTuru, @hesap, @tutar)";
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@islemTuru", "Para Çekme");
            cmd.Parameters.AddWithValue("@hesap", hesapNo);
            cmd.Parameters.AddWithValue("@tutar", -miktar);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void ParaYatir (int hesapNo, int miktar)
        {
            // para Yatırma işlemi
            SqlConnection con = new SqlConnection("server=DESKTOP-L799HNF\\SQLEXPRESS; Initial Catalog=test; Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "update hesap set hesapBakiye = hesapBakiye + @miktar where hesapNo = @hesapNo";
            cmd.Parameters.AddWithValue("@hesapNo", hesapNo);
            cmd.Parameters.AddWithValue("@miktar", miktar);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            // hesap özetine ekleme
            cmd.CommandText = "insert into islem values(@date, @islemTuru, @hesap, @tutar)";
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@islemTuru", "Para Yatırma");
            cmd.Parameters.AddWithValue("@hesap", hesapNo);
            cmd.Parameters.AddWithValue("@tutar", miktar);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void ParaAktar(int hesapNo, int hedefHesap, int miktar)
        {
            // hesaptan para düşme işlemi
            SqlConnection con = new SqlConnection("server=DESKTOP-L799HNF\\SQLEXPRESS; Initial Catalog=test; Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "update hesap set hesapBakiye = hesapBakiye - @miktar where hesapNo = @hesapNo";
            cmd.Parameters.AddWithValue("@hesapNo", hesapNo);
            cmd.Parameters.AddWithValue("@miktar", miktar);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            // hesap özetine ekleme
            string islemturu = Convert.ToString(hedefHesap) + " Hesaba Para Aktarma";
            cmd.CommandText = "insert into islem values(@date, @islemTuru, @hesap, @tutar)";
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@islemTuru",islemturu);
            cmd.Parameters.AddWithValue("@hesap", hesapNo);
            cmd.Parameters.AddWithValue("@tutar", -miktar);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            // hedef hesaba para yatırma işlemi
            cmd.Connection = con;
            cmd.CommandText = "update hesap set hesapBakiye = hesapBakiye + @miktar2 where hesapNo = @hedefHesap";
            cmd.Parameters.AddWithValue("@hedefHesap", hedefHesap);
            cmd.Parameters.AddWithValue("@miktar2", miktar);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            // hesap özetine ekleme
            islemturu = Convert.ToString(hesapNo) + " Hesaptan Gelen Para";
            cmd.CommandText = "insert into islem values(@date2, @islemTuru2, @hesap2, @tutar2)";
            cmd.Parameters.AddWithValue("@date2", DateTime.Now);
            cmd.Parameters.AddWithValue("@islemTuru2", islemturu);
            cmd.Parameters.AddWithValue("@hesap2", hedefHesap);
            cmd.Parameters.AddWithValue("@tutar2", miktar);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static bool HesapVarmi(int hesapNo)
        {
            SqlConnection con = new SqlConnection("server=DESKTOP-L799HNF\\SQLEXPRESS; Initial Catalog=test; Integrated Security=True");
            string sorgu = "select count(*) from hesap where hesapNo = @hesapNo";
            SqlCommand cmd = new SqlCommand(sorgu, con);
            cmd.Parameters.AddWithValue("@hesapNo", hesapNo);
            con.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            if (a == 1) { return true; }
            else { return false; }

        }

    }
}
