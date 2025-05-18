using FlipazonApi.Controllers;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Services.Interface;
using FlipazonApi.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;

namespace FlipazonApiTest.ControllerTest
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _cartServiceMock = new Mock<ICartService>();
            _controller = new OrderController(_orderServiceMock.Object, _cartServiceMock.Object);
        }

        [Fact]
        public async Task PlaceOrder_ReturnsSuccessResponse()
        {
            var request = new PlaceOrderRequest
            {
                UserId = 1,
                Items = [new() { ProductId = 101, Quantity = 2, Price = 200 }]
            };

            var expectedJson = new JsonResult(new { Message = "Order Placed Successfully!" });

            _orderServiceMock.Setup(s => s.PlaceOrderAsync(request)).ReturnsAsync(expectedJson);
            _cartServiceMock.Setup(s => s.RemoveOrderedItemsAsync(request.UserId, request.Items)).Returns(Task.CompletedTask);

            var result = await _controller.PlaceOrder(request) as JsonResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Order Placed Successfully!", data["Message"]);
        }

        [Fact]
        public async Task PlaceOrder_ThrowsHttpResponseException_ReturnsProperStatus()
        {
            var request = new PlaceOrderRequest { UserId = 2, Items = [] };
            _orderServiceMock.Setup(s => s.PlaceOrderAsync(request))
                .ThrowsAsync(new HttpResponseException(400, "Invalid order"));

            var result = await _controller.PlaceOrder(request) as JsonResult;

            Assert.Equal(400, result?.StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Invalid order", data["Message"]);
        }

        [Fact]
        public async Task PlaceOrder_ThrowsUnhandledException_Returns500()
        {
            var request = new PlaceOrderRequest { UserId = 3, Items = [] };
            _orderServiceMock.Setup(s => s.PlaceOrderAsync(request))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _controller.PlaceOrder(request) as JsonResult;

            Assert.Equal(500, result?.StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Database error", data["Message"]);
        }

        [Fact]
        public async Task GetOrdersByUserId_ReturnsOrderList()
        {
            string[] value = ["Order1", "Order2"];
            var expected = new JsonResult(value);

            _orderServiceMock.Setup(s => s.GetOrdersByUserIdAsync(1))
                .ReturnsAsync(expected);

            var result = await _controller.GetOrdersByUserId(1) as JsonResult;

            Assert.NotNull(result?.Value);
            var data = JArray.FromObject(result.Value);
            Assert.Contains("Order1", data);
        }

        [Fact]
        public async Task GetOrdersByUserId_ThrowsHttpResponseException_Returns404()
        {
            _orderServiceMock.Setup(s => s.GetOrdersByUserIdAsync(99))
                .ThrowsAsync(new HttpResponseException(404, "User not found"));

            var result = await _controller.GetOrdersByUserId(99) as JsonResult;

            Assert.Equal(404, result?.StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("User not found", data["Message"]);
        }

        [Fact]
        public async Task GetOrdersByUserId_ThrowsUnhandledException_Returns500()
        {
            _orderServiceMock.Setup(s => s.GetOrdersByUserIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _controller.GetOrdersByUserId(5) as JsonResult;

            Assert.Equal(500, result?.StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Unexpected error", data["Message"]);
        }
    }
}
