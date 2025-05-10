using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Services.Interface;
using FlipazonApi.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController(ICartService cartService) : ControllerBase
    {
        private readonly ICartService _cartService = cartService;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartItems(int userId)
        {
            try
            {
                return await _cartService.GetCartItemsByUserIdAsync(userId);
            }
            catch (HttpResponseException ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddOrUpdateCartItem([FromBody] CartItemRequest cartItemRequest)
        {
            try
            {
                return await _cartService.AddOrUpdateCartItemAsync(cartItemRequest.UserId, cartItemRequest.ProductId, cartItemRequest.Quantity);
            }
            catch (HttpResponseException ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }

    }

}
