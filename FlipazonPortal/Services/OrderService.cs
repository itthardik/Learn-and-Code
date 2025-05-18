using FlipazonPortal.Models;
using System.Net;

namespace FlipazonPortal.Services
{
    public class OrderService : ApiService
    {
        public static async Task<ResponseMessage> PlaceOrder(int userId, List<OrderItemDto> items)
        {
            try
            {
                ResponseMessage res = await SendRequest(() => new ()
                {
                    Url = "/api/order/place",
                    Method = HttpMethod.Post,
                    Body = new PlaceOrderRequest
                    {
                        UserId = userId,
                        Items = items
                    }
                });

                return res;
            }
            catch (Exception ex)
            {
                return new() { Message = $"Error placing order: {ex.Message}", StatusCode = HttpStatusCode.BadRequest };
            }
        }


        public static async Task<ResponseMessage> GetOrdersByUserId(int userId)
        {
            try
            {
                ResponseMessage res = await SendRequest(() => new ()
                {
                    Url = $"/api/order/{userId}",
                    Method = HttpMethod.Get
                });

                return res;
            }
            catch (Exception ex)
            {
                return new() { Message = $"Error retrieving orders: {ex.Message}", StatusCode = HttpStatusCode.BadRequest };
            }
        }
    }
}
