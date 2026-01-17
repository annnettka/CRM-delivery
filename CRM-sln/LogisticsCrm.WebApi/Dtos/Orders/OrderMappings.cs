using LogisticsCrm.Domain.Entities;

namespace LogisticsCrm.WebApi.Dtos.Orders
{
    public static class OrderMappings
    {
        public static OrderResponseDto ToDto(this Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                ClientId = order.ClientId,
                TrackingNumber = order.TrackingNumber,
                PickupAddress = order.PickupAddress,
                DeliveryAddress = order.DeliveryAddress,
                RecipientName = order.RecipientName,
                RecipientPhone = order.RecipientPhone,
                Price = order.Price,
                Status = (int)order.Status,
                CreatedAtUtc = order.CreatedAtUtc
            };
        }
    }
}
