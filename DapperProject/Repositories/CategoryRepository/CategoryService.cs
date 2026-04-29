using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.CategoryDtos;

namespace DapperProject.Repositories.CategoryRepository
{
    public class CategoryService : ICategoryService
    {
        private readonly DapperContext _context;

        public CategoryService(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            string query = "Select * From Categories";
            using var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultCategoryDto>(query);
            return values.ToList();
        }

        public async Task<GetByIdCategoryDto> GetByIdCategoryAsync(int id)
        {
            string query = "Select * From Categories Where CategoryId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using var connection = _context.CreateConnection();
            var value = await connection.QueryFirstAsync<GetByIdCategoryDto>(query, parameters);
            return value;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            string query = "Insert Into Categories (CategoryName, Status) Values (@categoryName, @status)";
            var parameters = new DynamicParameters();
            parameters.Add("@categoryName", createCategoryDto.CategoryName);
            parameters.Add("@status", createCategoryDto.Status);
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            string query = "Update Categories Set CategoryName = @categoryName, Status = @status Where CategoryId = @categoryId";
            var parameters = new DynamicParameters();
            parameters.Add("@categoryId", updateCategoryDto.CategoryId);
            parameters.Add("@categoryName", updateCategoryDto.CategoryName);
            parameters.Add("@status", updateCategoryDto.Status);
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            string query = "Delete From Categories Where CategoryId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
        public async Task<int> GetTotalCategoryAsync()
        {
            string query = "SELECT ISNULL(COUNT(*), 0) FROM Categories";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<int>(query);
        }

        public async Task<int> GetActiveCategoryAsync()
        {
            string query = "SELECT ISNULL(COUNT(*), 0) FROM Categories WHERE Status = 1";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<int>(query);
        }

        public async Task<int> GetPassiveCategoryAsync()
        {
            string query = "SELECT ISNULL(COUNT(*), 0) FROM Categories WHERE Status = 0";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<int>(query);
        }

        public async Task<ResultCategoryDto> GetLastCategoryAsync()
        {
            string query = "SELECT TOP 1 CategoryId, CategoryName, Status FROM Categories ORDER BY CategoryId DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ResultCategoryDto>(query);
        }
    }
}
