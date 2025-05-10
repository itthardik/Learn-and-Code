using FlipazonApi.DatabaseContext;
using FlipazonApi.Models;
using FlipazonApi.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FlipazonApi.Repository
{
    public class OrderRepository(FlipazonContext context) : IOrderRepository
    {
        private readonly FlipazonContext _context = context;

        public async Task<Order> PlaceOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }
    }
}
