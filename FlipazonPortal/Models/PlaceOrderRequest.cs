namespace FlipazonPortal.Models
{
    public class PlaceOrderRequest
    {
        public int UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = [];
    }
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
