using LogisticsCrm.Domain.Enums;

namespace LogisticsCrm.Domain.Entities
{
    public class OrderStatusHistory
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public OrderStatus FromStatus { get; private set; }
        public OrderStatus ToStatus { get; private set; }
        public DateTime ChangedAtUtc { get; private set; }
        public string? Comment { get; private set; }

        private OrderStatusHistory() { } // EF

        public OrderStatusHistory(Guid orderId, OrderStatus fromStatus, OrderStatus toStatus, string? comment)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            FromStatus = fromStatus;
            ToStatus = toStatus;
            Comment = comment;
            ChangedAtUtc = DateTime.UtcNow;
        }
    }
}
