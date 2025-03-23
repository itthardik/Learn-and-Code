using FlipazonApi.Models;

namespace FlipazonApi.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<Order> PlaceOrderAsync(Order order);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
    }
}
