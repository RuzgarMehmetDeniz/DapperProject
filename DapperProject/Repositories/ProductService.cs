using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.ProductDtos;

namespace DapperProject.Repositories
{
    public class ProductService : IProductService
    {
        private readonly DapperContext _context;

        public ProductService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            string query = "Insert Into Products (ProductName,Stock,Price,CategoryId) Values (@productname,@stock,@price,@categoryId)";
            var parameters = new DynamicParameters();
            parameters.Add("@productname", createProductDto.ProductName);
            parameters.Add("@stock", createProductDto.Stock);
            parameters.Add("@price", createProductDto.Price);
            parameters.Add("@categoryId", createProductDto.CategoryId);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteProductAsync(int productId)
        {
            string query = "Delete From Product Where ProductId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", productId);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            string query = "Select * From Products";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultProductDto>(query);
            return values.ToList();
        }

        public async Task<GetProrudctDto> GetProductByIdAsync(int productId)
        {
            string query = "Select * From Products Where ProductId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", productId);
            var connection = _context.CreateConnection();
            var values = await connection.QueryFirstAsync<GetProrudctDto>(query, parameters);
            return values;
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
          string query= "Update Products Set ProductName=@productname,Stock=@stock,Price=@price,CategoryId=@categoryId Where ProductId=@productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productId", updateProductDto.ProductId);
            parameters.Add("@productname", updateProductDto.ProductName);
            parameters.Add("@stock", updateProductDto.Stock);
            parameters.Add("@price", updateProductDto.Price);
            parameters.Add("@categoryId", updateProductDto.CategoryId);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
