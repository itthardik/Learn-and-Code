using FlipazonApi.Models.DTO.RequestDTO;

namespace FlipazonApi.Models.DTO
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = null!;

    }
}
