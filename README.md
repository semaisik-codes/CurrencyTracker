# CurrencyTracker
Bu proje, Frankfurter API kullanarak anlık döviz kurlarını (Türk Lirası bazlı) çeken, hadızada tutan ve kullanıcıya çeşitli LINQ sorgulama imkanları sunan bir C# konsol uygulamasıdır.
## Projenin Amacı
Finansal verilerin canlı olarak internetten çekilmesi, JSON formatındaki verinin C# nesnelerine dönüştürülmesi ve bu veriler üzerinde filtreleme, sıralama ve istatiksel hesaplamaların yapılması amaçlanmıştır.
## Özellikler ve Menü İşlevleri
Uygulama açıldığında API'den verileri çeker ve aşağıdaki işlemleri sunar: 
1. *Tüm Dövizleri Listele:* Tüm para birimlerini ve kurlarını listeler (LINQ Select).
2. *Koda Göre Ara:* Kulanıcının girdiği döviz koduna (örn: USD, EUR) göre arama yapar (LINQ Where).
3. *Değere Göre Filtrele:* Belirli bir değerden yüksek olan kurları listeler (LINQ Where).
4. *Sıralama:* Dövizleri değerine göre küükten büyüğe sıralar (LINQ OrderBy).
5. *İstatistiksel Özet:* Toplam döbiz sayısı, en yüksek/en düşük kur ve ortalama kur bilgisini gösterir (LINQ Count, Max, Min, Average)

## Kullanılan Teknolohiler ve Teknik Gereksinimler
Proje, aşağıdaki teknik şartlara tam uyumlulukla geliştirilmiştir.
- Dil: C# (.NET)
- Proje Tipi: Konsol Uygulaması
- Veri Erişimi: HttpClient ve async/await yapısı ile asenkron veri çekimi
- Veri Formatı: JSON verisi Newtonsoft.Json kütüphanesi
- Veri Yapısı: Dictionar olarak gelen veri, işlenebilir List<Currency yapısına dönüştürülmüştür.
- Sorgulama: Tüm veri işlemleri LINQ metotları ile yapılmıştır (döngü ile manuel filtreleme yapılmamıştır).

## Kullanılan API
Veriler Frankfurter API üzerinden **TRY (TÜRK Lirası)** bazlı olarak çekilmektedir.
* **URL:** `https://api.frankfurter.app/latest?from=TRY`
