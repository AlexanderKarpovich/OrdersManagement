namespace Ordering.API.Application.Commands.Dtos
{
    public class OrderDetailsDto
    {
        public int Id { get; init; }
        public string? Number { get; init; }
        public DateTime Date { get; init; }
        public int ProviderId { get; init; }
        public IEnumerable<OrderItemDto> OrderItems { get; init; } = default!;

        public static OrderDetailsDto FromOrder(Order order)
        {
            return new OrderDetailsDto()
            {
                Id = order.Id,
                Number = order.Number,
                Date = order.Date,
                ProviderId = order.ProviderId,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto(oi.Name!, oi.Quantity, oi.Unit!))
            };
        }
    }
}