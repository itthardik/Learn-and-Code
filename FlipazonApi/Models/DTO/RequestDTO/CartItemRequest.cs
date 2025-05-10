namespace FlipazonApi.Models.DTO.RequestDTO
{
    public class CartItemRequest
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
