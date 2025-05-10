using FlipazonPortal.Models;
using System.Net;

namespace FlipazonPortal.Services
{
    public class ProductService : ApiService
    {
        public static async Task<ResponseMessage> GetAllProducts()
        {
            try
            {
                ResponseMessage res = await SendRequest(() => new ()
                {
                    Url = "/api/category",
                    Method = HttpMethod.Get
                });
                return res;
            }
            catch (Exception ex)
            {
                return new() { Message = $"Error during login: {ex.Message}", StatusCode=HttpStatusCode.BadRequest };
            }
        }

        public static async Task<ResponseMessage> GetProductsByCategoryId(int id)
        {
            try
            {
                ResponseMessage res = await SendRequest(() => new ()
                {
                    Url = $"/api/category/{id}",
                    Method = HttpMethod.Get
                });
                return res;
            }
            catch (Exception ex)
            {
                return new() { Message = $"Error during login: {ex.Message}", StatusCode = HttpStatusCode.BadRequest };
            }
        }
    }
}
