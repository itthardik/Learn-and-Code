using FlipazonApi.Models.DTO;
using FlipazonApi.Models.DTO.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Services.Interface
{
    public interface IOrderService
    {
        Task<JsonResult> PlaceOrderAsync(PlaceOrderRequest request);
        Task<JsonResult> GetOrdersByUserIdAsync(int userId);
    }
}
