namespace Ordering.API.Application.Commands.Dtos
{
    public record OrderItemDto
    {
        public OrderItemDto(string name, decimal quantity, string unit)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.Unit = unit;
        }

        public string? Name { get; init; }
        public decimal Quantity { get; init; }
        public string? Unit { get; init; }
    }
}