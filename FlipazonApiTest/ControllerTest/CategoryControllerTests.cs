using FlipazonApi.Controllers;
using FlipazonApi.Services.Interface;
using FlipazonApi.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;

namespace FlipazonApiTest.ControllerTest
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoryController(_categoryServiceMock.Object);
        }

        [Fact]
        public async Task GetCategories_ReturnsCategoryList()
        {
            string[] value = ["Electronics", "Books"];
            var expected = new JsonResult(value);

            _categoryServiceMock.Setup(s => s.GetAllCategories())
                                .ReturnsAsync(expected);

            var result = await _controller.GetCategories() as JsonResult;

            Assert.NotNull(result?.Value);
            var data = JArray.FromObject(result.Value);
            Assert.Contains("Books", data);
        }

        [Fact]
        public async Task GetCategories_ThrowsHttpResponseException_ReturnsProperStatus()
        {
            _categoryServiceMock.Setup(s => s.GetAllCategories())
                                .ThrowsAsync(new HttpResponseException(404, "Categories not found"));

            var result = await _controller.GetCategories() as JsonResult;

            Assert.Equal(404, result?.StatusCode);
            Assert.NotNull(result?.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Categories not found", data["Message"]);
        }

        [Fact]
        public async Task GetCategories_ThrowsUnhandledException_Returns500()
        {
            _categoryServiceMock.Setup(s => s.GetAllCategories())
                                .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _controller.GetCategories() as JsonResult;

            Assert.Equal(500, result?.StatusCode);
            Assert.NotNull(result?.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Unexpected error", data["Message"]);
        }

        [Fact]
        public async Task GetProductsByCategoryId_ReturnsProductList()
        {
            var categoryId = 1;
            string[] value = ["Laptop", "Mouse"];
            var expected = new JsonResult(value);

            _categoryServiceMock.Setup(s => s.GetProductsByCategoryId(categoryId))
                                .ReturnsAsync(expected);

            var result = await _controller.GetProductsByCategoryId(categoryId) as JsonResult;

            Assert.NotNull(result?.Value);
            var data = JArray.FromObject(result.Value);
            Assert.Contains("Laptop", data);
        }

        [Fact]
        public async Task GetProductsByCategoryId_ThrowsHttpResponseException_Returns404()
        {
            _categoryServiceMock.Setup(s => s.GetProductsByCategoryId(It.IsAny<int>()))
                                .ThrowsAsync(new HttpResponseException(404, "Category not found"));

            var result = await _controller.GetProductsByCategoryId(5) as JsonResult;

            Assert.Equal(404, result?.StatusCode);
            Assert.NotNull(result?.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Category not found", data["Message"]);
        }

        [Fact]
        public async Task GetProductsByCategoryId_ThrowsUnhandledException_Returns500()
        {
            _categoryServiceMock.Setup(s => s.GetProductsByCategoryId(It.IsAny<int>()))
                                .ThrowsAsync(new Exception("Something went wrong"));

            var result = await _controller.GetProductsByCategoryId(5) as JsonResult;

            Assert.Equal(500, result?.StatusCode);
            Assert.NotNull(result?.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Something went wrong", data["Message"]);
        }
    }
}
