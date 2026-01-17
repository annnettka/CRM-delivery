namespace LogisticsCrm.WebApi.Dtos.Orders
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public string PickupAddress { get; set; } = null!;
        public string DeliveryAddress { get; set; } = null!;
        public string RecipientName { get; set; } = null!;
        public string? RecipientPhone { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public Guid? CourierId { get; set; }


    }
}
