using FlipazonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FlipazonApi.DatabaseContext
{
    public class FlipazonContext(DbContextOptions<FlipazonContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}