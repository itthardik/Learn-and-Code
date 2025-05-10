using FlipazonApi.Models;

namespace FlipazonApi.Repository.Interface
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllCategories();
        public Task<Category?> GetProductsByCategoryIdAsync(int categoryId);
    }
}
