using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq; // LINQ sorguları için 
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyTracker
{
    // zorunlu model sınıfları

    // API'den gelen ana yapı
    public class CurrencyResponse
    {
        public string Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    // liste içinde kullanacağım sadeleştirilmiş sınıf
    public class Currency
    {
        public string Code { get; set; } // Örn: USD, EUR
        public decimal Rate { get; set; } // Örn: 0.03
    }

    internal class Program
    {
        // Dövizleri tutacağım ana liste
        static List<Currency> dovizListesi = new List<Currency>();

        static async Task Main(string[] args)
        {
            // 1. verileri listeye dönüştür
            await VerileriGetir();

            if (dovizListesi.Count == 0)
            {
                Console.WriteLine("Veri çekilemedi, program kapatılıyor.");
                return;
            }

            // 2. menü döngüsü
            bool devam = true;
            while (devam)
            {
                Console.WriteLine("\n=====CurrencyTracker=====");
                Console.WriteLine("1. Tüm dövizleri listele");
                Console.WriteLine("2. Koda göre döviz ara");
                Console.WriteLine("3. Belirli bir değerden büyük dövizleri listele");
                Console.WriteLine("4. Dövizleri değere göre sırala (Küçükten Büyüğe)");
                Console.WriteLine("5. İstatistiksel özet göster");
                Console.WriteLine("0. Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine();
                Console.WriteLine("============================");

                switch (secim)
                {
                    case "1":
                        Listele();
                        break;
                    case "2":
                        Ara();
                        break;
                    case "3":
                        DegerFiltrele();
                        break;
                    case "4":
                        Sirala();
                        break;
                    case "5":
                        Istatistik();
                        break;
                    case "0":
                        devam = false;
                        Console.WriteLine("Çıkış yapılıyor...");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        break;
                }
            }
        }

        static async Task VerileriGetir()
        {
            Console.WriteLine("Veriler Frankfurter API'den çekiliyor...");
            string url = "https://api.frankfurter.app/latest?from=TRY";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);

                    // JSON'u dictionary yapısına uygun modele çevirme
                    CurrencyResponse response = JsonConvert.DeserializeObject<CurrencyResponse>(json);

                    // dictionary verisini (key-value), list<currency> formatına dönüştürme (LINQ sorguları list)
                    foreach (var item in response.Rates)
                    {
                        dovizListesi.Add(new Currency
                        {
                            Code = item.Key,
                            Rate = item.Value
                        });
                    }
                    Console.WriteLine("Veriler başarıyla yüklendi! Hafızaya alındı.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }

        // 1️⃣ tüm dövizleri listele (LINQ Select)
        static void Listele()
        {
            Console.WriteLine("Tüm Döviz Kurları (Baz: TRY)");

            // LINQ Select ile veriyi formatlayıp seç
            var liste = dovizListesi.Select(x => $"{x.Code}: {x.Rate}");

            foreach (var satir in liste)
            {
                Console.WriteLine(satir);
            }
        }

        // 2️⃣ koda göre döviz ara (LINQ Where)
        static void Ara()
        {
            Console.Write("Döviz Kodu Girin (Örn: USD, EUR): ");
            string kod = Console.ReadLine().ToUpper(); // Büyük harf
            // LINQ Where ile filtreleme
            var sonuc = dovizListesi.Where(x => x.Code == kod).FirstOrDefault();

            if (sonuc != null)
            {
                Console.WriteLine($"Bulundu: 1 TRY = {sonuc.Rate} {sonuc.Code}");
            }
            else
            {
                Console.WriteLine("Böyle bir para birimi bulunamadı.");
            }
        }

        // 3️⃣ belirli bir değerden büyük dövizler (LINQ Where)
        static void DegerFiltrele()
        {
            Console.Write("Hangi değerden büyükleri görelim? (Örn: 0.5): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal limit))
            {
                // LINQ Where kullanımı
                var sonuclar = dovizListesi.Where(x => x.Rate > limit).ToList();

                if (sonuclar.Count > 0)
                {
                    Console.WriteLine($"\n{limit} değerinden büyük {sonuclar.Count} adet para birimi var:");
                    foreach (var item in sonuclar)
                    {
                        Console.WriteLine($"{item.Code} - {item.Rate}");
                    }
                }
                else
                {
                    Console.WriteLine("Bu kriterde döviz bulunamadı.");
                }
            }
            else
            {
                Console.WriteLine("Geçerli bir sayı girmediniz!");
            }
        }

        // 4️⃣ Dövizleri Değere Göre Sırala (LINQ OrderBy)
        static void Sirala()
        {
            Console.WriteLine("Değere Göre Sıralı Liste (Küçükten Büyüğe)");

            // LINQ OrderBy kullanımı
            var siraliListe = dovizListesi.OrderBy(x => x.Rate).ToList();

            foreach (var item in siraliListe)
            {
                Console.WriteLine($"{item.Code}: {item.Rate}");
            }
        }

        // 5️⃣ istatistiksel özet (LINQ Count, Max, Min, Average)
        static void Istatistik()
        {
            Console.WriteLine("İstatistikler");

            // LINQ Aggragate (toplama/özet) fonksiyonları
            int toplamSayi = dovizListesi.Count();
            decimal enYuksek = dovizListesi.Max(x => x.Rate);
            decimal enDusuk = dovizListesi.Min(x => x.Rate);
            decimal ortalama = dovizListesi.Average(x => x.Rate);

            // en yüksek ve en düşük olanın isimlerini de bul
            var enYuksekDoviz = dovizListesi.First(x => x.Rate == enYuksek);
            var enDusukDoviz = dovizListesi.First(x => x.Rate == enDusuk);

            Console.WriteLine($"Toplam Para Birimi Sayısı : {toplamSayi}");
            Console.WriteLine($"Ortalama Kur Değeri       : {Math.Round(ortalama, 4)}");
            Console.WriteLine($"En Yüksek Kur             : {enYuksek} ({enYuksekDoviz.Code})");
            Console.WriteLine($"En Düşük Kur              : {enDusuk} ({enDusukDoviz.Code})");
        }
    }
}