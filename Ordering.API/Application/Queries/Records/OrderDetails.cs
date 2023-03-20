namespace Ordering.API.Application.Queries.Records
{
    public record OrderDetails
    {
        public OrderDetails(OrderSummary orderSummary, IEnumerable<OrderItemSummary> orderItems)
        {
            Id = orderSummary.Id;
            Number = orderSummary.Number;
            Date = orderSummary.Date;
            ProviderId = orderSummary.ProviderId;
            OrderItems = orderItems;
        }

        public int Id { get; init; }
        public string? Number { get; init; }
        public DateTime Date { get; init; }
        public int ProviderId { get; init; }

        public IEnumerable<OrderItemSummary> OrderItems { get; init; }
    }
}