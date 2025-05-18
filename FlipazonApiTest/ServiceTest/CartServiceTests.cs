using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Repository.Interface;
using FlipazonApi.Services;
using Moq;
using Newtonsoft.Json.Linq;

namespace FlipazonApiTest.ServiceTest
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _cartService = new CartService(_cartRepositoryMock.Object);
        }

        [Fact]
        public async Task GetCartItemsByUserIdAsync_ReturnsJsonResult()
        {
            var userId = 1;
            var cartItems = new List<CartItem>
            {
                new() { UserId = userId, ProductId = 101, Quantity = 2 },
                new() { UserId = userId, ProductId = 102, Quantity = 1 }
            };

            _cartRepositoryMock.Setup(repo => repo.GetCartItemsByUserIdAsync(userId))
                               .ReturnsAsync(cartItems);

            var result = await _cartService.GetCartItemsByUserIdAsync(userId);

            Assert.NotNull(result.Value);
            var data = JArray.FromObject(result.Value);
            Assert.Equal(2, data.Count);
            Assert.Equal(101, data[0]["ProductId"]);
        }

        [Fact]
        public async Task AddOrUpdateCartItemAsync_UpdatesExistingItem_ReturnsSuccessMessage()
        {
            var userId = 1;
            var productId = 101;
            var quantity = 2;
            var existingItem = new CartItem { UserId = userId, ProductId = productId, Quantity = 1 };

            _cartRepositoryMock.Setup(repo => repo.GetCartItemAsync(userId, productId))
                               .ReturnsAsync(existingItem);

            var result = await _cartService.AddOrUpdateCartItemAsync(userId, productId, quantity);

            _cartRepositoryMock.Verify(r => r.UpdateCartItemAsync(It.Is<CartItem>(c => c.Quantity == 3)), Times.Once);
            _cartRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Cart fetched successfully", data["Message"]);
        }

        [Fact]
        public async Task AddOrUpdateCartItemAsync_AddsNewItem_ReturnsSuccessMessage()
        {
            var userId = 2;
            var productId = 202;
            var quantity = 4;

            _cartRepositoryMock.Setup(repo => repo.GetCartItemAsync(userId, productId))
                               .ReturnsAsync((CartItem?)null);

            var result = await _cartService.AddOrUpdateCartItemAsync(userId, productId, quantity);

            _cartRepositoryMock.Verify(r => r.AddCartItemAsync(It.Is<CartItem>(
                c => c.UserId == userId && c.ProductId == productId && c.Quantity == quantity
            )), Times.Once);

            _cartRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Cart fetched successfully", data["Message"]);
        }

        [Fact]
        public async Task RemoveOrderedItemsAsync_CallsRepositoryMethods()
        {
            var userId = 1;
            var orderItems = new List<OrderItemDto>
            {
                new OrderItemDto { ProductId = 101, Quantity = 1 },
                new OrderItemDto { ProductId = 102, Quantity = 2 }
            };

            await _cartService.RemoveOrderedItemsAsync(userId, orderItems);

            _cartRepositoryMock.Verify(r => r.RemoveOrderedItemsAsync(userId, orderItems), Times.Once);
            _cartRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
