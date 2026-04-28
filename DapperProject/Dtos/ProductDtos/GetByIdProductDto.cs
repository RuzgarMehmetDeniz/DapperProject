namespace DapperProject.Dtos.ProductDtos
{
    public class GetByIdProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int CategoryID { get; set; }
        public int Price { get; set; }
        public string Brand { get; set; }
    }
}
