using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.OrderDtos;
using DapperProject.Models.ML;

namespace DapperProject.Repositories.OrderRepository
{
    public class OrderService : IOrderService
    {
        private readonly DapperContext _context;

        public OrderService(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<ResultOrderDto>> GetAllOrderAsync()
        {
            string query = @"
        SELECT 
            o.OrderId,
            o.ProductId,
            p.Name                   AS ProductName,
            o.CustomerId,
            c.Name + ' ' + c.Surname AS CustomerName,
            cat.CategoryName,
            o.Quantity,
            p.Price,
            o.Quantity * p.Price     AS TotalPrice,
            o.Status,
            o.OrderDate
        FROM Orders o
        LEFT JOIN Product    p   ON o.ProductId  = p.ProductID
        LEFT JOIN Customers  c   ON o.CustomerId = c.CustomerId
        LEFT JOIN Categories cat ON p.CategoryID = cat.CategoryId";

            using var connection = _context.CreateConnection();
            var command = new CommandDefinition(query, commandTimeout: 120);
            var values = await connection.QueryAsync<ResultOrderDto>(command);
            return values.ToList();
        }

        public async Task<GetByIdOrderDto> GetByIdOrderAsync(int id)
        {
            string query = "SELECT OrderId, ProductId, CustomerId, Quantity, Price, Status, OrderDate FROM Orders WHERE OrderId = @OrderId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<GetByIdOrderDto>(query, new { OrderId = id });
        }

        public async Task CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            string query = @"INSERT INTO Orders (ProductId, CustomerId, Quantity, Price, Status)
                             VALUES (@ProductId, @CustomerId, @Quantity, @Price, @Status)";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, createOrderDto);
        }

        public async Task UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            string query = @"UPDATE Orders SET
                                ProductId  = @ProductId,
                                CustomerId = @CustomerId,
                                Quantity   = @Quantity,
                                Price      = @Price,
                                Status     = @Status
                             WHERE OrderId = @OrderId";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, updateOrderDto);
        }

        public async Task DeleteOrderAsync(int id)
        {
            string query = "DELETE FROM Orders WHERE OrderId = @OrderId";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { OrderId = id });
        }
        public async Task<int> GetTotalOrderCountAsync()
        {
            string query = "SELECT COUNT(*) FROM Orders";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            string query = @"
                SELECT SUM(o.Quantity * p.Price)
                FROM Orders o
                LEFT JOIN Product p ON o.ProductId = p.ProductID";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<decimal>(query);
        }

        public async Task<TopCustomerDto> GetTopCustomerAsync()
        {
            string query = @"
                SELECT TOP 1
                    c.Name + ' ' + c.Surname AS Name,
                    COUNT(o.OrderId)          AS OrderCount
                FROM Orders o
                LEFT JOIN Customers c ON o.CustomerId = c.CustomerId
                GROUP BY c.Name, c.Surname
                ORDER BY OrderCount DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TopCustomerDto>(query);
        }

        public async Task<LastOrderDto> GetLastOrderAsync()
        {
            string query = @"
                SELECT TOP 1
                    o.OrderId,
                    c.Name + ' ' + c.Surname AS CustomerName
                FROM Orders o
                LEFT JOIN Customers c ON o.CustomerId = c.CustomerId
                ORDER BY o.OrderId DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<LastOrderDto>(query);
        }
        public async Task<List<OrderYearlyData>> GetYearlyOrderDataAsync()
        {
            string query = @"
        SELECT 
            YEAR(o.OrderDate)            AS Year,
            SUM(o.Quantity * p.Price)    AS TotalRevenue,
            COUNT(o.OrderId)             AS TotalOrders
        FROM Orders o
        LEFT JOIN Product p ON o.ProductId = p.ProductID
        GROUP BY YEAR(o.OrderDate)
        ORDER BY Year";

            using var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<OrderYearlyData>(query);
            return values.ToList();
        }
    }
}