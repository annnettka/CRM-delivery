using LogisticsCrm.Domain.Enums;

namespace LogisticsCrm.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }

        public Guid ClientId { get; private set; }

        public string TrackingNumber { get; private set; } = null!;

        public string PickupAddress { get; private set; } = null!;
        public string DeliveryAddress { get; private set; } = null!;

        public string RecipientName { get; private set; } = null!;
        public string? RecipientPhone { get; private set; }

        public decimal Price { get; private set; }
        public OrderStatus Status { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        private Order() { } // EF

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

        public void Assign() => Status = OrderStatus.Assigned;
        public void MarkPickedUp() => Status = OrderStatus.PickedUp;
        public void MarkInTransit() => Status = OrderStatus.InTransit;
        public void MarkDelivered() => Status = OrderStatus.Delivered;
        public void Cancel() => Status = OrderStatus.Canceled;
    }
}
