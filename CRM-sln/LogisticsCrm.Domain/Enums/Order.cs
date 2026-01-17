using LogisticsCrm.Domain.Enums;
using LogisticsCrm.Domain.Rules;

namespace LogisticsCrm.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? CourierId { get; private set; }

        public string TrackingNumber { get; private set; } = null!;
        public string PickupAddress { get; private set; } = null!;
        public string DeliveryAddress { get; private set; } = null!;
        public string RecipientName { get; private set; } = null!;
        public string? RecipientPhone { get; private set; }

        public decimal Price { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        private Order() { }

        public Order(
            Guid clientId,
            string trackingNumber,
            string pickupAddress,
            string deliveryAddress,
            string recipientName,
            string? recipientPhone,
            decimal price)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;

            TrackingNumber = trackingNumber ?? throw new ArgumentNullException(nameof(trackingNumber));
            PickupAddress = pickupAddress ?? throw new ArgumentNullException(nameof(pickupAddress));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            RecipientName = recipientName ?? throw new ArgumentNullException(nameof(recipientName));
            RecipientPhone = recipientPhone;

            Price = price;
            Status = OrderStatus.Created;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void AssignCourier(Guid courierId)
        {
            if (courierId == Guid.Empty)
                throw new ArgumentException("CourierId cannot be empty.", nameof(courierId));

            if (Status is OrderStatus.Delivered or OrderStatus.Canceled)
                throw new InvalidOperationException("Cannot assign courier for delivered or canceled order.");

            CourierId = courierId;
        }

        public void ChangeStatus(OrderStatus newStatus)
        {
            if (!OrderStatusTransitions.CanMove(Status, newStatus))
                throw new InvalidOperationException($"Invalid status transition: {Status} -> {newStatus}");

            if (newStatus == OrderStatus.Assigned && CourierId == null)
                throw new InvalidOperationException("Cannot assign order without courier.");

            Status = newStatus;
        }
    }
}
