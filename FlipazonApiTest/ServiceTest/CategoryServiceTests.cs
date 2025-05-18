using FlipazonApi.Models;
using FlipazonApi.Repository.Interface;
using FlipazonApi.Services;
using Moq;
using Newtonsoft.Json.Linq;

namespace FlipazonApiTest.ServiceTest
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsListOfCategories()
        {
            var categories = new List<Category>
            {
                new() { Id = 1, Name = "Electronics" },
                new() { Id = 2, Name = "Books" }
            };

            _categoryRepositoryMock.Setup(repo => repo.GetAllCategories())
                                   .ReturnsAsync(categories);

            var result = await _categoryService.GetAllCategories();

            Assert.NotNull(result.Value);
            var data = JArray.FromObject(result.Value);
            Assert.Equal(2, data.Count);
            Assert.Equal("Electronics", data[0]["Name"]);
        }

        [Fact]
        public async Task GetProductsByCategoryId_ValidId_ReturnsProducts()
        {
            var categoryId = 1;
            var categoryProducts = new Category
            {
                Id = categoryId,
                Name = "Clothing",
                Products =
                [
                    new() { Id = 101, Name = "Shirt" },
                    new() { Id = 102, Name = "Jeans" }
                ]
            };

            _categoryRepositoryMock.Setup(repo => repo.GetProductsByCategoryIdAsync(categoryId))
                                   .ReturnsAsync(categoryProducts);

            var result = await _categoryService.GetProductsByCategoryId(categoryId);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Clothing", data["Name"]);
            Assert.Equal(2, data["Products"]?.Count());
        }

        [Fact]
        public async Task GetProductsByCategoryId_InvalidId_Returns404()
        {
            var invalidCategoryId = 99;

            _categoryRepositoryMock.Setup(repo => repo.GetProductsByCategoryIdAsync(invalidCategoryId))
                                   .ReturnsAsync((Category?)null);

            var result = await _categoryService.GetProductsByCategoryId(invalidCategoryId);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Category not found", data["Message"]);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
