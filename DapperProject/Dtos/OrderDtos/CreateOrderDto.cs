namespace DapperProject.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}