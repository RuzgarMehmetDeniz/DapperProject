namespace DapperProject.Dtos.OrderDtos
{
    public class GetByIdOrderDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
    }
}