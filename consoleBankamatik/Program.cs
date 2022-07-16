using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleBankamatik
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string ad;
            int bakiye, sifre, aktifHesap, islemdekiPara, aktarmaHesap;
        // ana menü karşılaması
        mainMenu:
            metod.AraEkle();
            Console.WriteLine("Yapmak İstediğiniz işlemi Seçiniz!");
            Console.WriteLine("1:   Yeni Hesap Aç");
            Console.WriteLine("2:   Hesabıma Giriş Yap");
            Console.WriteLine("****Press Any Button To Exit!***");
            string pressed = Console.ReadLine();

            switch(pressed)
            {
                case "1": goto createAccount;
                case "2": goto enterAccount;
                default:  goto end;
            }


        // hesap oluşturulması
        createAccount:
            metod.AraEkle();
            Console.WriteLine("Lütfen Adınızı Giriniz:");
            ad = Console.ReadLine();
            Console.WriteLine("Lütfen Şifre Belirleyin:");
            sifre = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Lütfen Başlangıç Bakiyeyi Belirleyin:");
            bakiye = Convert.ToInt32(Console.ReadLine());
            metod.HesapEkle(ad, sifre, bakiye);
            Console.WriteLine("{0} Hesabınız Başarıyla Oluşturuldu!", ad);
            goto mainMenu;


        enterAccount:
            metod.AraEkle();
            Console.Write("Hesap Numaranızı Giriniz: ");
            int hesap = Convert.ToInt32(Console.ReadLine());
            Console.Write("Şifrenizi Giriniz: ");
            int sfr = Convert.ToInt32(Console.ReadLine());

            if (metod.HesapDorula(hesap, sfr) == true)
            { 
                Console.WriteLine("Giriş Başarılı!");
                aktifHesap = hesap;
                metod.AraEkle();
                goto accountMenu;
            }
            else 
            { 
                Console.WriteLine("Hatalı Giriş!");
                metod.AraEkle();
                goto mainMenu;
            }

        // giriş yapıldıktan sonraki kullanıcıyı karşılayan menu
        accountMenu:
            metod.AraEkle();
            Console.WriteLine("HOŞ GELDİNİZ " + metod.HesapIsmi(aktifHesap));
            Console.WriteLine("Bakiyeniz: " + metod.Bakiye(aktifHesap));
            Console.WriteLine("1:   Para Çek");
            Console.WriteLine("2:   Para Yatır");
            Console.WriteLine("3:   Para Aktar");
            Console.WriteLine("4:   Hesap Özeti");
            Console.WriteLine("5:   Çıkış Yap");
            Console.WriteLine("****Press Any Button To Exit!***");
            string Apressed = Console.ReadLine();

            switch (Apressed)
            {
                // para çekme işlemleri
                case "1":
                    metod.AraEkle();
                    Console.WriteLine("Bakiyeniz: " + metod.Bakiye(aktifHesap));
                    Console.Write("Çekmek İstediğiniz Tutarı Giriniz: ");
                    islemdekiPara = Convert.ToInt32(Console.ReadLine());
                    if (islemdekiPara > metod.Bakiye(aktifHesap)) // bakiye sorgulanıp çekilen para fazla ise işlem yapılmaz
                    {
                        Console.WriteLine("Yetersiz Bakiye");
                    }
                    else
                    {
                        metod.ParaCek(aktifHesap, islemdekiPara);
                        islemdekiPara = 0;
                        Console.WriteLine("İşlem Başarılı"); 
                    }
                    Console.ReadLine();
                    goto accountMenu;

                // para yatırma işlemleri  
                case "2":
                    
                    metod.AraEkle();
                    Console.Write("Yatırmak İstediğiniz Tutarı Giriniz: ");
                    islemdekiPara = Convert.ToInt32(Console.ReadLine());
                  
                    metod.ParaYatir(aktifHesap, islemdekiPara);
                    islemdekiPara = 0;
                    Console.WriteLine("İşlem Başarılı");
                    Console.ReadLine();
                    goto accountMenu;

                // Para aktarma işlemleri
                case "3":
                    metod.AraEkle();
                    Console.WriteLine("Bakiyeniz: " + metod.Bakiye(aktifHesap));
                    Console.Write("Aktarmak İstediğiniz Tutarı Giriniz: ");
                    islemdekiPara = Convert.ToInt32(Console.ReadLine());

                    if (islemdekiPara > metod.Bakiye(aktifHesap)) // bakiye sorgulanıp çekilen para fazla ise işlem yapılmaz
                    {
                        Console.WriteLine("Yetersiz Bakiye");
                    }
                    else
                    {
                        Console.Write("Aktarmak İstediğiniz Hesap No Giriniz: ");
                        aktarmaHesap = Convert.ToInt32(Console.ReadLine());
                        if (metod.HesapVarmi(aktarmaHesap))
                        {
                            metod.ParaAktar(aktifHesap, aktarmaHesap, islemdekiPara);
                            Console.WriteLine("İşlem Başarılı!");
                            islemdekiPara = 0;
                            aktarmaHesap = 0;
                        }
                        else
                        {
                            Console.WriteLine("Hesap Bulunamadı!");
                        }
                    }

                    Console.ReadLine();
                    goto accountMenu;

                // hesap özeti işlemleri  
                case "4":

                    metod.AraEkle();
                    Console.WriteLine("Tarih      Saat      Miktar  İşlem Türü");
                    Console.WriteLine("---------------------------------------------");
                    SqlConnection con = new SqlConnection("server=DESKTOP-L799HNF\\SQLEXPRESS; Initial Catalog=test; Integrated Security=True");
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader dr;
                    cmd.Connection = con;
                    cmd.CommandText = "select * from islem where islemHesap = @hesapNo";
                    cmd.Parameters.AddWithValue("@hesapNo", aktifHesap);
                    con.Open();
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Console.WriteLine
                            (dr[1] + "   " + dr[4] + "   " + dr[2]);
                    }
                    con.Close();                    

                    Console.ReadLine();
                    goto accountMenu;

                // hesaptan çıkış işlemleri
                case "5":
                    aktifHesap = 0;
                    goto mainMenu;

                default: goto end;
            }

        end:
            Console.ReadLine();
        }
    }
}
