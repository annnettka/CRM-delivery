using LogisticsCrm.Domain.Enums;

namespace LogisticsCrm.Domain.Rules;

public static class OrderStatusTransitions
{
    private static readonly Dictionary<OrderStatus, OrderStatus[]> Allowed = new()
    {
        { OrderStatus.Created,   new[] { OrderStatus.Assigned, OrderStatus.Canceled } },
        { OrderStatus.Assigned,  new[] { OrderStatus.PickedUp, OrderStatus.Canceled } },
        { OrderStatus.PickedUp,  new[] { OrderStatus.InTransit, OrderStatus.Canceled } },
        { OrderStatus.InTransit, new[] { OrderStatus.Delivered, OrderStatus.Canceled } },
        { OrderStatus.Delivered, Array.Empty<OrderStatus>() },
        { OrderStatus.Canceled,  Array.Empty<OrderStatus>() }
    };

    public static bool CanMove(OrderStatus from, OrderStatus to)
        => Allowed.TryGetValue(from, out var next) && next.Contains(to);
}
