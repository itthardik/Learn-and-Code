using FlipazonApi.Models;
using FlipazonApi.Models.DTO;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Repository.Interface;
using FlipazonApi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Services
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<JsonResult> PlaceOrderAsync(PlaceOrderRequest request)
        {
            var order = new Order
            {
                UserId = request.UserId,
                OrderItems = [.. request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                })],
                TotalAmount = request.Items.Sum(i => i.Price * i.Quantity),
            };

            order = await _orderRepository.PlaceOrderAsync(order);

            return new JsonResult(new
            {
                Message = "Order Placed Successfully!",
                Data = new OrderResponse
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Items = [.. order.OrderItems.Select(i => new OrderItemDto
                            {
                                ProductId = i.ProductId,
                                Quantity = i.Quantity,
                                Price = i.Price
                            })]
                }
            });
        }

        public async Task<JsonResult> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            if (orders.Count == 0)
                return new JsonResult(new { Message = "No orders found for this user." });

            return new JsonResult(new { 
                Message = "Order List",
                Data = orders
        });
        }
    }
}
