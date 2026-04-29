using DapperProject.Dtos.CategoryDtos;

namespace DapperProject.Repositories.CategoryRepository
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllCategoryAsync();
        Task<GetByIdCategoryDto> GetByIdCategoryAsync(int id);
        Task CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(int id);

        Task<int> GetTotalCategoryAsync();
        Task<int> GetActiveCategoryAsync();
        Task<int> GetPassiveCategoryAsync();
        Task<ResultCategoryDto> GetLastCategoryAsync();
    }
}