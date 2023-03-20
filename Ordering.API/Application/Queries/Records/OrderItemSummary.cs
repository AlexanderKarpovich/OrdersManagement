namespace Ordering.API.Application.Queries.Records
{
    public record OrderItemSummary
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public decimal Quantity { get; init; }
        public string? Unit { get; init; }
    }
}