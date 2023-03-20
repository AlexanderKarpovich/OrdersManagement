namespace Ordering.API.Application.Queries.Records
{
    public record OrderSummary
    {
        public int Id { get; init; }
        public string? Number { get; init; }
        public DateTime Date { get; init; }
        public int ProviderId { get; init; }
    }
}