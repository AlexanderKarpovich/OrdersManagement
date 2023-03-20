namespace Ordering.API.Application.Queries.Records
{
    public record ProviderSummary
    {
        public int Id { get; init; }
        public string? Name { get; init; }
    }
}