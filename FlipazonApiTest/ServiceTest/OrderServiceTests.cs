using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Repository.Interface;
using FlipazonApi.Services;
using Moq;
using Newtonsoft.Json.Linq;

namespace FlipazonApiTest.ServiceTest
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object);
        }

        [Fact]
        public async Task PlaceOrderAsync_ReturnsJsonResultWithOrderData()
        {
            var request = new PlaceOrderRequest
            {
                UserId = 1,
                Items =
                [
                    new() { ProductId = 101, Quantity = 2, Price = 50 },
                    new() { ProductId = 102, Quantity = 1, Price = 100 }
                ]
            };

            var placedOrder = new Order
            {
                OrderId = 1,
                UserId = 1,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 200,
                OrderItems =
                [
                    new() { ProductId = 101, Quantity = 2, Price = 50 },
                    new() { ProductId = 102, Quantity = 1, Price = 100 }
                ]
            };

            _orderRepositoryMock.Setup(repo => repo.PlaceOrderAsync(It.IsAny<Order>()))
                                .ReturnsAsync(placedOrder);

            var result = await _orderService.PlaceOrderAsync(request);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Order Placed Successfully!", data["Message"]);
            Assert.Equal(1, data["Data"]?["OrderId"]);
            Assert.Equal(200, data["Data"]?["TotalAmount"]);
            Assert.Equal(2, data["Data"]?["Items"]?.Count());
        }

        [Fact]
        public async Task GetOrdersByUserIdAsync_WithOrders_ReturnsOrderList()
        {
            var orders = new List<Order>
            {
                new() { OrderId = 1, UserId = 1, TotalAmount = 150 },
                new() { OrderId = 2, UserId = 1, TotalAmount = 300 }
            };

            _orderRepositoryMock.Setup(repo => repo.GetOrdersByUserIdAsync(1))
                                .ReturnsAsync(orders);

            var result = await _orderService.GetOrdersByUserIdAsync(1);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Order List", data["Message"]);
            Assert.Equal(2, data["Data"]?.Count());
        }

        [Fact]
        public async Task GetOrdersByUserIdAsync_NoOrders_ReturnsMessage()
        {
            _orderRepositoryMock.Setup(repo => repo.GetOrdersByUserIdAsync(1))
                                .ReturnsAsync([]);

            var result = await _orderService.GetOrdersByUserIdAsync(1);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("No orders found for this user.", data["Message"]);
            Assert.Null(data["Data"]);
        }
    }
}
