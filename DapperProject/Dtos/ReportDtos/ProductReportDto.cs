// ───────────────────────────────────────────
// DTO'lar - ReportController.cs altına ekle
// ───────────────────────────────────────────
public class ProductReportDto
{
    public int ProductID { get; set; }
    public string Name { get; set; } = "";
    public string Brand { get; set; } = "";
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = "";
}
public class CustomerReportDto
{
    public int CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}

public class OrderReportDto
{
    public int OrderId { get; set; }
    public string? CustomerName { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    public string? Status { get; set; }
    public DateTime OrderDate { get; set; }
}

public class CategoryReportDto
{
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int Status { get; set; }
    public int ProductCount { get; set; }
}