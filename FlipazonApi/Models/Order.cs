using System.ComponentModel.DataAnnotations;

namespace FlipazonApi.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public List<OrderItem> OrderItems { get; set; } = [];
    }
}
