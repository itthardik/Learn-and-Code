using FlipazonPortal.Models;
using System.Net;

namespace FlipazonPortal.Services
{
    public class CartService : ApiService
    {
        public static async Task<ResponseMessage> GetCartItemsByUserId(int userId)
        {
            try
            {
                ResponseMessage res = await SendRequest(() => new ()
                {
                    Url = $"/api/cart/{userId}",
                    Method = HttpMethod.Get
                });
                return res;
            }
            catch (Exception ex)
            {
                return new() { Message = $"Error retrieving cart items: {ex.Message}", StatusCode = HttpStatusCode.BadRequest };
            }
        }

        public static async Task<ResponseMessage> AddOrUpdateCartItem(CartItemRequest cartItemRequest)
        {
            try
            {
                ResponseMessage res = await SendRequest(() => new ()
                {
                    Url = "/api/cart/add",
                    Method = HttpMethod.Post,
                    Body = cartItemRequest
                });
                return res;
            }
            catch (Exception ex)
            {
                return new() { Message = $"Error updating cart: {ex.Message}", StatusCode = HttpStatusCode.BadRequest };
            }
        }
        
        public static async Task<ResponseMessage> ClearCart(int userId)
        {
            try
            {
                ResponseMessage res = await SendRequest(() => new ()
                {
                    Url = $"/api/cart/clear/{userId}",
                    Method = HttpMethod.Delete
                });

                return res;
            }
            catch (Exception ex)
            {
                return new() { Message = $"Error clearing cart: {ex.Message}", StatusCode = HttpStatusCode.BadRequest };
            }
        }

    }
}
