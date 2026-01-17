namespace LogisticsCrm.WebApi.Dtos.Orders
{
    public class GetOrdersQuery
    {
        public int? Status { get; set; }
        public Guid? CourierId { get; set; }
        public Guid? ClientId { get; set; }

        public string? Search { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
