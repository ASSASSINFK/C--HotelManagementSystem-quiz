using System;
using System.Collections.Generic;
using System.Linq;

namespace OtelSistemiSınav
{
    public class OdaBulunamadiException : Exception
    {
        public OdaBulunamadiException(string mesaj) : base(mesaj) { }
    }

    public class OdaMusaitDegilException : Exception
    {
        public OdaMusaitDegilException(string mesaj) : base(mesaj) { }
    }

    public class GecersizOdaTipiException : Exception
    {
        public GecersizOdaTipiException(string mesaj) : base(mesaj) { }
    }

    public interface IOtelIslemleri
    {
        void OdaEkle(Oda oda);
        void OdalariListele();
        void RezervasyonYap(int odaNo, int gunSayisi);
    }

    public class Oda
    {
        private int odaNo;
        private string odaTipi;
        private double fiyat;
        private bool musaitmi;

        public int OdaNo { get => odaNo; set => odaNo = value; }
        public string OdaTipi { get => odaTipi; set => odaTipi = value; }
        public double Fiyat { get => fiyat; set => fiyat = value; }
        public bool Musaitmi { get => musaitmi; set => musaitmi = value; }

        public Oda(int no, string tip)
        {
            this.odaNo = no;
            this.odaTipi = tip;
            this.musaitmi = true; 
        }
    }

    public class Otel : IOtelIslemleri
    {
        private List<Oda> odalar = new List<Oda>();

        public void OdaEkle(Oda oda)
        {
            switch (oda.OdaTipi.ToLower())
            {
                case "tek":
                case "single":
                    oda.Fiyat = 200;
                    break;
                case "çift":
                case "cift":
                case "double":
                    oda.Fiyat = 350;
                    break;
                case "suite":
                case "suit":
                    oda.Fiyat = 600;
                    break;
                default:
                    throw new GecersizOdaTipiException($"'{oda.OdaTipi}' tipi geçersizdir!");
            }
            odalar.Add(oda);
            Console.WriteLine($"{oda.OdaNo} nolu oda sisteme eklendi.");
        }

        public void OdalariListele()
        {
            Console.WriteLine("\n--- ODA LİSTESİ ---");
            foreach (var o in odalar)
            {
                Console.WriteLine($"No: {o.OdaNo} | Tip: {o.OdaTipi} | Fiyat: {o.Fiyat} | Durum: {(o.Musaitmi ? "Müsait" : "Dolu")}");
            }
        }

        public void RezervasyonYap(int odaNo, int gunSayisi)
        {
            Oda arananOda = odalar.FirstOrDefault(x => x.OdaNo == odaNo);

            if (arananOda == null)
                throw new OdaBulunamadiException($"{odaNo} numaralı oda sistemde kayıtlı değil!");

            if (!arananOda.Musaitmi)
                throw new OdaMusaitDegilException($"{odaNo} numaralı oda şu an dolu!");

            arananOda.Musaitmi = false;
            double toplam = arananOda.Fiyat * gunSayisi;
            Console.WriteLine($"\nRezervasyon Tamamlandı! Toplam Borç: {toplam} TL");
          
            Console.WriteLine("\n*******************************");
            Console.WriteLine("    REZERVASYON ONAYLANDI");
            Console.WriteLine("*******************************");
            Console.WriteLine($"Oda Numarası : {arananOda.OdaNo}");
            Console.WriteLine($"Oda Tipi     : {arananOda.OdaTipi}");
            Console.WriteLine($"Günlük Fiyat : {arananOda.Fiyat} TL");
            Console.WriteLine($"Kalınacak Gün: {gunSayisi}");
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"TOPLAM TUTAR : {toplam} TL");
            Console.WriteLine("*******************************\n");
        }
    }

   
    class Program
    {
        static void Main(string[] args)
        {
            Otel myOtel = new Otel();

            try
            {
               Oda zimmer= new Oda(101, "Tek");
              
                myOtel.OdaEkle(zimmer);
                myOtel.OdaEkle(new Oda(101, "Çift"));

                myOtel.OdalariListele();

             
                myOtel.RezervasyonYap(101, 3);
                myOtel.RezervasyonYap(101, 3);
                
                myOtel.RezervasyonYap(101, 2); 
                myOtel.OdaEkle(new Oda(105, "Kral Dairesi")); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n[HATA]: " + ex.Message);
            }

            Console.ReadKey();
        }
    }
}
