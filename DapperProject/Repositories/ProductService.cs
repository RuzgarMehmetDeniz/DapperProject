using DapperProject.Context;
using DapperProject.Dtos.ProductDtos;
using Dapper;

namespace DapperProject.Repositories
{
    public class ProductService : IProductService
    {
        private readonly DapperContext _context;

        public ProductService(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            string query = "SELECT ProductID, Name, Stock, CategoryID, Price, Brand FROM Product";
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultProductDto>(query);
            return result.ToList();
        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(int id)
        {
            string query = "SELECT ProductID, Name, Stock, CategoryID, Price, Brand FROM Product WHERE ProductID = @ProductID";
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<GetByIdProductDto>(query, new { ProductID = id });
            return result;
        }

        public async Task CreateProductAsync(CreateProductDto productDto)
        {
            string query = "INSERT INTO Product (Name, Stock, CategoryID, Price, Brand) VALUES (@Name, @Stock, @CategoryID, @Price, @Brand)";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, productDto);
        }

        public async Task UpdateProductAsync(UpdateProductDto productDto)
        {
            string query = "UPDATE Product SET Name = @Name, Stock = @Stock, CategoryID = @CategoryID, Price = @Price, Brand = @Brand WHERE ProductID = @ProductID";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, productDto);
        }

        public async Task DeleteProductAsync(int id)
        {
            string query = "DELETE FROM Product WHERE ProductID = @ProductID";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { ProductID = id });
        }
    }
}