<div align="center">

# 🧠 NexusBI

<br/>

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC_.NET_8-512BD4?style=for-the-badge&logo=dotnet)
![Dapper](https://img.shields.io/badge/Dapper-2.1.72-grey?style=for-the-badge)
![ML.NET](https://img.shields.io/badge/ML.NET-5.0-blue?style=for-the-badge&logo=microsoft)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![ClosedXML](https://img.shields.io/badge/ClosedXML-0.105.0-217346?style=for-the-badge&logo=microsoftexcel)

</div>

---

## 📌 Proje Hakkında

NexusBI, büyük ölçekli e-ticaret ve perakende işletmelerinin ihtiyaç duyduğu veri yönetimi, analiz ve tahminleme işlevlerini tek bir arayüz altında toplayan bir **Business Intelligence** uygulamasıdır.

Geleneksel yönetim panellerinin aksine NexusBI; yalnızca veri listelemekle kalmaz — aylık büyüme oranlarını hesaplar, stok risklerini görselleştirir, sipariş trendlerini grafiklere döker ve ML.NET altyapısıyla gelecek yıl tahminleri üretir.

Uygulama; **500.000 sipariş**, **1.500.000 ürün** ve **50.000 müşteri** içeren gerçekçi veri hacimleriyle test edilmiş ve bu ölçekte performanslı çalışacak biçimde tasarlanmıştır.

---

## 🛠️ Kullanılan Teknolojiler

| Alan | Teknoloji | Açıklama |
|------|-----------|----------|
| Web Framework | ASP.NET Core MVC (.NET 8) | Katmanlı mimari ve güçlü routing altyapısı |
| ORM | Dapper 2.1.72 | Ham SQL ile yüksek performanslı veri erişimi; büyük veri setlerinde Entity Framework'e kıyasla çok daha hızlı |
| Veritabanı | Microsoft SQL Server | Yüksek hacimli ilişkisel veri yönetimi |
| Makine Öğrenmesi | ML.NET 5.0 + TimeSeries | SSA (Singular Spectrum Analysis) algoritmasıyla zaman serisi tahmini |
| Excel Export | ClosedXML 0.105.0 | Sunucu tarafında anlık `.xlsx` üretimi ve kullanıcıya indirme |

---
- **Controller** — Yalnızca isteği alır ve ilgili servisi çağırır. İş mantığına karışmaz.
- **Service Layer** — Her entity için ayrı `IService` interface'i ve implementasyonu bulunur. Filtreleme, hesaplama gibi iş kuralları burada yazılır.
- **Repository** — Dapper sorguları bu katmanda tutulur. Servis katmanı veritabanına doğrudan erişmez.
- **DTO** — Veritabanından gelen ham veri View'a hiçbir zaman doğrudan taşınmaz; DTO sınıfları aracılığıyla şekillendirilir.
- **ViewComponent** — Sidebar, KPI kartları gibi birden fazla sayfada kullanılan UI parçaları ViewComponent olarak izole edilmiştir.

---

## 📂 Proje Yapısı
## 🏗️ Mimari

Proje, **Repository Pattern** ve **Service Layer** mimarisi üzerine inşa edilmiştir. Her katmanın tek ve net bir sorumluluğu vardır; katmanlar birbirinden bağımsız çalışır.
---

## 🖥️ Uygulama Sayfaları

---

### 📊 Dashboard

KPI kartlarında toplam sipariş, aktif kategori, aylık ciro, toplam müşteri, aylık büyüme oranı ve bugünkü sipariş sayısı gerçek zamanlı olarak gösterilir. Popüler kategorilerin bar grafiği, haftalık sipariş trendi ile en yüksek ve en düşük stoklu ürünler de aynı sayfada listelenir.

<img width="1363" height="1209" alt="Dashboard" src="https://github.com/user-attachments/assets/e83069b5-9d44-412c-8787-2bf758316399" />

---

### 📦 Ürün Yönetimi

1.500.000'den fazla ürün sayfalanarak listelenir. Her kayıt için stok miktarı progress bar ile görselleştirilmiş, fiyat, marka ve kategori bilgileri yan yana sunulmuştur.

<img width="1365" height="891" alt="Product" src="https://github.com/user-attachments/assets/d7ff951b-67cf-44b4-92d7-225f40aec5f3" />

---

### 👥 Müşteri Yönetimi

50.000'den fazla müşteri ad, soyad, şehir ve ülke bilgileriyle listelenir. Sayfanın üstünde toplam müşteri sayısı, farklı şehir adedi ve en yoğun bölge gibi özet kartlar yer alır.

<img width="1365" height="873" alt="Customer" src="https://github.com/user-attachments/assets/01c738e5-c87a-4f9b-aca1-7853e33ee4fa" />

---

### 🛒 Sipariş Yönetimi

500.000 siparişin listelendiği bu sayfada her satır; müşteri adı, ürün adı, kategori, adet, toplam fiyat, durum rozeti ve tarih bilgisini içerir. Sipariş durumları 🟢 Kargoda · 🟡 Beklemede · 🔴 İptal Edildi olarak ayrıştırılmıştır.

<img width="1365" height="762" alt="Order" src="https://github.com/user-attachments/assets/01a77064-4f0e-45dd-a1b1-e7f443fd4ab3" />

---

### 🗂️ Kategori Yönetimi

110 kategorinin aktif/pasif durumu bu sayfadan yönetilir. Her kategoriye kaç ürün bağlı olduğu anlık olarak gösterilir; toplam, aktif, pasif sayıları ve son eklenen kategori üst kartlarda özetlenir.

<img width="1365" height="873" alt="Category" src="https://github.com/user-attachments/assets/39a07c30-c2ab-4442-83a2-149de9613902" />

---

### 🤖 Sipariş Tahminleme — ML.NET

2022, 2023 ve 2024 yıllarına ait gerçek sipariş adedi ve ciro verileri, ML.NET'in SSA algoritmasıyla analiz edilerek 2025 ve 2026 yılı tahminleri üretilir. Gerçek veri ile tahmini veri aynı grafik üzerinde karşılaştırmalı olarak sunulur.

<img width="1355" height="1002" alt="ML1" src="https://github.com/user-attachments/assets/a31517b0-01c2-4c1f-ad07-b96935e97515" />

---

### 📁 Excel Raporları

Müşteri, Ürün, Sipariş ve Kategori verileri tek tıkla `.xlsx` formatında indirilebilir. Raporlar anlık SQL sorgusuyla çekilir, ClosedXML ile bellekte Excel dosyasına dönüştürülür ve kullanıcıya sunulur.

<img width="453" height="697" alt="CustomerExcel" src="https://github.com/user-attachments/assets/70121efa-5f3d-4eae-a4bf-6ade4dc1c4cc" />
