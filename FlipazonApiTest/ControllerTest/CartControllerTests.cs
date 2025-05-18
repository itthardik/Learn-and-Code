using FlipazonApi.Controllers;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Services.Interface;
using FlipazonApi.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;

namespace FlipazonApiTest.ControllerTest
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly CartController _cartController;

        public CartControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _cartController = new CartController(_cartServiceMock.Object);
        }

        [Fact]
        public async Task GetCartItems_ReturnsJsonResult()
        {
            var userId = 1;
            string[] items = ["item1", "item2"];
            var expected = new JsonResult(new { Items = items });

            _cartServiceMock.Setup(s => s.GetCartItemsByUserIdAsync(userId))
                            .ReturnsAsync(expected);

            var result = await _cartController.GetCartItems(userId) as JsonResult;

            Assert.NotNull(result?.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("item1", data["Items"]?[0]);
        }

        [Fact]
        public async Task GetCartItems_HttpResponseException_ReturnsProperStatusCode()
        {
            var ex = new HttpResponseException(404, "User not found");

            _cartServiceMock.Setup(s => s.GetCartItemsByUserIdAsync(It.IsAny<int>()))
                            .ThrowsAsync(ex);

            var result = await _cartController.GetCartItems(1) as JsonResult;

            Assert.Equal(404, result?.StatusCode);
            Assert.NotNull(result?.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("User not found", data["Message"]);
        }

        [Fact]
        public async Task GetCartItems_UnhandledException_Returns500()
        {
            _cartServiceMock.Setup(s => s.GetCartItemsByUserIdAsync(It.IsAny<int>()))
                            .ThrowsAsync(new Exception("Unexpected"));

            var result = await _cartController.GetCartItems(1) as JsonResult;

            Assert.Equal(500, result?.StatusCode);
            Assert.NotNull(result?.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Unexpected", data["Message"]);
        }

        [Fact]
        public async Task AddOrUpdateCartItem_ReturnsSuccessJson()
        {
            var request = new CartItemRequest { UserId = 1, ProductId = 101, Quantity = 2 };
            var expected = new JsonResult(new { Message = "Cart updated" });

            _cartServiceMock.Setup(s => s.AddOrUpdateCartItemAsync(request.UserId, request.ProductId, request.Quantity))
                            .ReturnsAsync(expected);

            var result = await _cartController.AddOrUpdateCartItem(request) as JsonResult;

            Assert.NotNull(result?.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Cart updated", data["Message"]);
        }

        [Fact]
        public async Task AddOrUpdateCartItem_HttpResponseException_ReturnsProperStatusCode()
        {
            var request = new CartItemRequest { UserId = 1, ProductId = 101, Quantity = 2 };
            var ex = new HttpResponseException(400, "Invalid product");

            _cartServiceMock.Setup(s => s.AddOrUpdateCartItemAsync(request.UserId, request.ProductId, request.Quantity))
                            .ThrowsAsync(ex);

            var result = await _cartController.AddOrUpdateCartItem(request) as JsonResult;

            Assert.Equal(400, result?.StatusCode);
            Assert.NotNull(result?.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Invalid product", data["Message"]);
        }

        [Fact]
        public async Task AddOrUpdateCartItem_UnhandledException_Returns500()
        {
            var request = new CartItemRequest { UserId = 1, ProductId = 101, Quantity = 2 };

            _cartServiceMock.Setup(s => s.AddOrUpdateCartItemAsync(request.UserId, request.ProductId, request.Quantity))
                            .ThrowsAsync(new Exception("Unexpected failure"));

            var result = await _cartController.AddOrUpdateCartItem(request) as JsonResult;

            Assert.Equal(500, result?.StatusCode);
            Assert.NotNull(result?.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Unexpected failure", data["Message"]);
        }
    }
}
