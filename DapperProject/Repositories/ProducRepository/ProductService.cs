using DapperProject.Context;
using DapperProject.Dtos.ProductDtos;
using Dapper;
namespace DapperProject.Repositories.ProductRepository.ProductService
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
        public async Task<int> GetTotalStockAsync()
        {
            string query = "SELECT SUM(Stock) FROM Product";
            using var connection = _context.CreateConnection();
            var command = new CommandDefinition(query, commandTimeout: 120);
            var value = await connection.QueryFirstAsync<int>(command);
            return value;
        }

        public async Task<ResultProductDto> GetMaxStockProductAsync()
        {
            string query = "Select Top 1 * From Product Order By Stock DESC";
            using var connection = _context.CreateConnection();
            var value = await connection.QueryFirstAsync<ResultProductDto>(query);
            return value;
        }

        public async Task<ResultProductDto> GetMinStockProductAsync()
        {
            string query = "Select Top 1 * From Product Order By Stock ASC";
            using var connection = _context.CreateConnection();
            var value = await connection.QueryFirstAsync<ResultProductDto>(query);
            return value;
        }

        public async Task<ResultProductDto> GetLastAddedProductAsync()
        {
            string query = "Select Top 1 * From Product Order By ProductID DESC";
            using var connection = _context.CreateConnection();
            var value = await connection.QueryFirstAsync<ResultProductDto>(query);
            return value;
        }
    }
}