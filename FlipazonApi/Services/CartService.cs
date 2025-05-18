using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Repository.Interface;
using FlipazonApi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Services
{
    public class CartService(ICartRepository cartRepository) : ICartService
    {
        private readonly ICartRepository _cartRepository = cartRepository;

        public async Task<JsonResult> GetCartItemsByUserIdAsync(int userId)
        {
            var cartItems =  await _cartRepository.GetCartItemsByUserIdAsync(userId);
            return new JsonResult(cartItems);
        }

        public async Task<JsonResult> AddOrUpdateCartItemAsync(int userId, int productId, int quantity)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                _cartRepository.UpdateCartItemAsync(cartItem);
            }
            else
            {
                cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _cartRepository.AddCartItemAsync(cartItem);
            }

            await _cartRepository.SaveChangesAsync();
            return new JsonResult( new { Message = "Cart fetched successfully"});
        }

        public async Task RemoveOrderedItemsAsync(int userId, List<OrderItemDto> orderItems)
        {
            await _cartRepository.RemoveOrderedItemsAsync(userId, orderItems);
            await _cartRepository.SaveChangesAsync();
        }

    }
}