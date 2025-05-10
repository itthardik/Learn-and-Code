using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Repository.Interface;
using FlipazonApi.Services.Interface;
using FlipazonApi.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace FlipazonApi.Services
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<JsonResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return new JsonResult(categories);
        }

        public async Task<JsonResult> GetProductsByCategoryId(int categoryId)
        {
            var category = await _categoryRepository.GetProductsByCategoryIdAsync(categoryId);
            if (category == null)
                return new JsonResult(new { Message = "Category not found" }) { StatusCode = 404 };

            return new JsonResult(category);
        }
    }
}
