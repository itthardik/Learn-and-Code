using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Services.Interface
{
    public interface ICategoryService
    {
        public Task<JsonResult> GetAllCategories();
        public Task<JsonResult> GetProductsByCategoryId(int categoryId);
    }
}
