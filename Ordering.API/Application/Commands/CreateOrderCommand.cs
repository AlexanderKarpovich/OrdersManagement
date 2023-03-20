namespace Ordering.API.Application.Commands
{
    [DataContract]
    public class CreateOrderCommand : IRequest<OrderDetailsDto>
    {
        public CreateOrderCommand()
        {
            OrderItems = new List<OrderItemDto>();
        }

        public CreateOrderCommand(string number, DateTime date, int providerId, IEnumerable<OrderItemDto> orderItems)
            : this()
        {
            Number = number;
            Date = date;
            ProviderId = providerId;
            OrderItems = orderItems;
        }

        [DataMember]
        public string? Number { get; init; }

        [DataMember]
        public DateTime Date { get; init; }

        [DataMember]
        public int ProviderId { get; init; }

        [DataMember]
        public IEnumerable<OrderItemDto> OrderItems { get; init; }
    }
}