using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Drawing;

namespace DapperProject.Controllers
{
    public class ReportController : Controller
    {
        private readonly string _connectionString;

        public ReportController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("connectionkey");
        }

        // ───────────────────────────────────────────
        // MÜŞTERİ RAPORU
        // ───────────────────────────────────────────
        public IActionResult Customer()
        {
            using var connection = new SqlConnection(_connectionString);
            var data = connection.Query(@"
                SELECT CustomerId, Name, Surname, City, Country
                FROM Customers
            ").ToList();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Müşteriler");

            // Başlıklar
            ws.Cell(1, 1).Value = "Müşteri ID";
            ws.Cell(1, 2).Value = "Ad";
            ws.Cell(1, 3).Value = "Soyad";
            ws.Cell(1, 4).Value = "Şehir";
            ws.Cell(1, 5).Value = "Ülke";

            // Başlık stili
            var headerRange = ws.Range("A1:E1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#1e293b");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Veriler
            int row = 2;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = (int)item.CustomerId;
                ws.Cell(row, 2).Value = (string)item.Name;
                ws.Cell(row, 3).Value = (string)item.Surname;
                ws.Cell(row, 4).Value = (string)(item.City ?? "");
                ws.Cell(row, 5).Value = (string)(item.Country ?? "");
                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Musteri_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        // ───────────────────────────────────────────
        // ÜRÜN RAPORU
        // ───────────────────────────────────────────
        public IActionResult Product()
        {
            using var connection = new SqlConnection(_connectionString);

            var data = connection.Query<ProductReportDto>(@"
        SELECT 
            p.ProductID,
            p.Name,
            p.Brand,
            p.Stock,
            p.Price,
            ISNULL(c.CategoryName, '') AS CategoryName
        FROM Product p
        LEFT JOIN Categories c ON p.CategoryID = c.CategoryId
    ").AsList();

            // Kaç kayıt geldi konsola yaz
            System.Diagnostics.Debug.WriteLine($"Ürün sayısı: {data.Count}");

            var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Urünler"); // Türkçe karakter YOK

            string[] headers = { "Ürün ID", "Ürün Adı", "Marka", "Stok", "Fiyat", "Kategori" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(1, i + 1).Value = headers[i];
            }

            var headerRange = ws.Range(1, 1, 1, headers.Length);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#1e293b");
            headerRange.Style.Font.FontColor = XLColor.White;

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                int row = i + 2; // 0-based index + 2 (başlık satırı)

                ws.Cell(row, 1).Value = item.ProductID;
                ws.Cell(row, 2).Value = item.Name ?? "";
                ws.Cell(row, 3).Value = item.Brand ?? "";
                ws.Cell(row, 4).Value = item.Stock;
                ws.Cell(row, 5).Value = item.Price;
                ws.Cell(row, 6).Value = item.CategoryName ?? "";
            }

            ws.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            workbook.Dispose();
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Urun_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
        }        // ───────────────────────────────────────────
        // SİPARİŞ RAPORU
        // ───────────────────────────────────────────
        public IActionResult Order()
        {
            using var connection = new SqlConnection(_connectionString);
            var data = connection.Query(@"
                SELECT o.OrderId, 
                       c.Name + ' ' + c.Surname AS CustomerName,
                       p.Name AS ProductName,
                       o.Quantity,
                       o.Price,
                       o.Quantity * o.Price AS Total,
                       o.Status,
                       o.OrderDate
                FROM Orders o
                LEFT JOIN Customers c ON o.CustomerId = c.CustomerId
                LEFT JOIN Product p ON o.ProductId = p.ProductID
            ").ToList();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Siparişler");

            ws.Cell(1, 1).Value = "Sipariş ID";
            ws.Cell(1, 2).Value = "Müşteri";
            ws.Cell(1, 3).Value = "Ürün";
            ws.Cell(1, 4).Value = "Adet";
            ws.Cell(1, 5).Value = "Birim Fiyat (₺)";
            ws.Cell(1, 6).Value = "Toplam (₺)";
            ws.Cell(1, 7).Value = "Durum";
            ws.Cell(1, 8).Value = "Tarih";

            var headerRange = ws.Range("A1:H1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#1e293b");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            foreach (var item in data)
            {
                string statusLabel = ((string)(item.Status ?? "")).ToLower() switch
                {
                    "shipped" => "Kargoya Verildi",
                    "pending" => "Beklemede",
                    "cancelled" => "İptal Edildi",
                    _ => (string)(item.Status ?? "")
                };

                ws.Cell(row, 1).Value = (int)item.OrderId;
                ws.Cell(row, 2).Value = (string)(item.CustomerName ?? "");
                ws.Cell(row, 3).Value = (string)(item.ProductName ?? "");
                ws.Cell(row, 4).Value = (int)item.Quantity;
                ws.Cell(row, 5).Value = (decimal)item.Price;
                ws.Cell(row, 6).Value = (decimal)item.Total;
                ws.Cell(row, 7).Value = statusLabel;
                ws.Cell(row, 8).Value = ((DateTime)item.OrderDate).ToString("dd.MM.yyyy");
                row++;
            }

            ws.Column(5).Style.NumberFormat.Format = "#,##0.00 ₺";
            ws.Column(6).Style.NumberFormat.Format = "#,##0.00 ₺";
            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Siparis_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        // ───────────────────────────────────────────
        // KATEGORİ RAPORU
        // ───────────────────────────────────────────
        public IActionResult Category()
        {
            using var connection = new SqlConnection(_connectionString);
            var data = connection.Query(@"
                SELECT c.CategoryId, 
                       c.CategoryName, 
                       c.Status,
                       COUNT(p.ProductID) AS ProductCount
                FROM Categories c
                LEFT JOIN Product p ON p.CategoryID = c.CategoryId
                GROUP BY c.CategoryId, c.CategoryName, c.Status
            ").ToList();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Kategoriler");

            ws.Cell(1, 1).Value = "Kategori ID";
            ws.Cell(1, 2).Value = "Kategori Adı";
            ws.Cell(1, 3).Value = "Durum";
            ws.Cell(1, 4).Value = "Ürün Sayısı";

            var headerRange = ws.Range("A1:D1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#1e293b");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            foreach (var item in data)
            {
                string statusLabel = (int)item.Status == 1 ? "Aktif" : "Pasif";

                ws.Cell(row, 1).Value = (int)item.CategoryId;
                ws.Cell(row, 2).Value = (string)item.CategoryName;
                ws.Cell(row, 3).Value = statusLabel;
                ws.Cell(row, 4).Value = (int)item.ProductCount;
                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Kategori_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
        }
    }
}