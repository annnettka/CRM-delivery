namespace LogisticsCrm.WebApi.Dtos.Orders
{
    public class CreateOrderRequest
    {
        public Guid ClientId { get; set; }
        public string PickupAddress { get; set; } = null!;
        public string DeliveryAddress { get; set; } = null!;
        public string RecipientName { get; set; } = null!;
        public string? RecipientPhone { get; set; }
        public decimal Price { get; set; }
    }
}
