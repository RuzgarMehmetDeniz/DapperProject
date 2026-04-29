using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.CustomerDtos;

namespace DapperProject.Repositories.CustomerRepository
{
    public class CustomerService : ICustomerService
    {
        private readonly DapperContext _context;

        public CustomerService(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<ResultCustomerDto>> GetAllCustomerAsync()
        {
            string query = "Select * From Customers";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultCustomerDto>(query);
                return values.ToList();
            }
        }

        public async Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            string query = "Insert into Customers (Name, Surname, City, Country) values (@name, @surname, @city, @country)";
            var parameters = new DynamicParameters();
            parameters.Add("@name", createCustomerDto.Name);
            parameters.Add("@surname", createCustomerDto.Surname);
            parameters.Add("@city", createCustomerDto.City);
            parameters.Add("@country", createCustomerDto.Country);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            string query = "Update Customers Set Name=@name, Surname=@surname, City=@city, Country=@country where CustomerId=@customerId";
            var parameters = new DynamicParameters();
            parameters.Add("@name", updateCustomerDto.Name);
            parameters.Add("@surname", updateCustomerDto.Surname);
            parameters.Add("@city", updateCustomerDto.City);
            parameters.Add("@country", updateCustomerDto.Country);
            parameters.Add("@customerId", updateCustomerDto.CustomerId);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            string query = "Delete From Customers Where CustomerId=@customerId";
            var parameters = new DynamicParameters();
            parameters.Add("@customerId", id);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<GetByIdCustomerDto> GetByIdCustomerAsync(int id)
        {
            string query = "Select * From Customers Where CustomerId=@customerId";
            var parameters = new DynamicParameters();
            parameters.Add("@customerId", id);

            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<GetByIdCustomerDto>(query, parameters);
                return value;
            }
        }
        public async Task<int> GetTotalCustomerCountAsync()
        {
            string query = "Select Count(*) From Customers";
            using (var connection = _context.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(query);
                return count;
            }
        }

        public async Task<int> GetCityCountAsync()
        {
            string query = "Select Count(Distinct(City)) From Customers";
            using (var connection = _context.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(query);
                return count;
            }
        }

        public async Task<ResultCustomerDto> GetLastCustomerAsync()
        {
            string query = "Select Top(1) * From Customers Order By CustomerId Desc";
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<ResultCustomerDto>(query);
                return value;
            }
        }

        public async Task<string> GetTopCityNameAsync()
        {
            string query = "Select Top(1) City From Customers Group By City Order By Count(*) Desc";
            using (var connection = _context.CreateConnection())
            {
                var cityName = await connection.ExecuteScalarAsync<string>(query);
                return cityName;
            }
        }
    }
}
