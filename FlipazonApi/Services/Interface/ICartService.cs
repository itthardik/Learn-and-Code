using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Services.Interface
{
    public interface ICartService
    {
        Task<JsonResult> GetCartItemsByUserIdAsync(int userId);
        Task<JsonResult> AddOrUpdateCartItemAsync(int userId, int productId, int quantity);
        Task RemoveOrderedItemsAsync(int userId, List<OrderItemDto> orderItems);
    }

}
