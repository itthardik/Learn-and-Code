using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Repository.Interface
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int userId, int productId);
        Task AddCartItemAsync(CartItem cartItem);
        void UpdateCartItemAsync(CartItem cartItem);
        Task RemoveOrderedItemsAsync(int userId, List<OrderItemDto> orderItems);
        Task SaveChangesAsync();
    }
}
