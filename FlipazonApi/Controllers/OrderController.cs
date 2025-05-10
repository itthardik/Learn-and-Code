using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Services;
using FlipazonApi.Services.Interface;
using FlipazonApi.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController(IOrderService orderService, ICartService cartService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ICartService _cartService = cartService;

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            try
            {
                var res = await _orderService.PlaceOrderAsync(request);
                await _cartService.RemoveOrderedItemsAsync(request.UserId, request.Items);
                return res;
            }
            
            catch (HttpResponseException ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message}) { StatusCode = 500 };
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            try
            {
                return await _orderService.GetOrdersByUserIdAsync(userId);
            }
            catch (HttpResponseException ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message}) { StatusCode = 500 };
            }
        }
    }
}
