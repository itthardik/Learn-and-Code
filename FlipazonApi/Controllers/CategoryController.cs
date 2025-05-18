using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Services.Interface;
using FlipazonApi.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                return await _categoryService.GetAllCategories();
            }
            catch (HttpResponseException ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message}) { StatusCode = 500 };
            }
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            try
            {
                return await _categoryService.GetProductsByCategoryId(categoryId);
            }
            catch (HttpResponseException ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }
    }

}
