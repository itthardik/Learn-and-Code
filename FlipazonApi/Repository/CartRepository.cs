using FlipazonApi.DatabaseContext;
using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FlipazonApi.Repository
{
    public class CartRepository(FlipazonContext context) : ICartRepository
    {
        private readonly FlipazonContext _context = context;

        public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _context.CartItems.Where(c => c.UserId == userId).Include(c=>c.Product).ToListAsync();
        }

        public async Task<CartItem?> GetCartItemAsync(int userId, int productId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }

        public void UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
        }

        public async Task RemoveOrderedItemsAsync(int userId, List<OrderItemDto> orderItems)
        {
            var productIds = orderItems.Select(o => o.ProductId).ToList();
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId && productIds.Contains(c.ProductId))
                .ToListAsync();

            if (cartItems.Count != 0)
            {
                _context.CartItems.RemoveRange(cartItems);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }

}
