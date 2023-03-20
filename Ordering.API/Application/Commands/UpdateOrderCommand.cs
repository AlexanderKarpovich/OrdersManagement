namespace Ordering.API.Application.Commands
{
    [DataContract]
    public class UpdateOrderCommand : IRequest<bool>
    {
        public UpdateOrderCommand()
        {
            this.OrderItems = new List<OrderItemDto>();
        }

        public UpdateOrderCommand(int orderId, string number, DateTime date, int providerId, List<OrderItemDto> orderItems)
            : this()
        {
            this.OrderId = orderId;
            this.Number = number;
            this.Date = date;
            this.ProviderId = providerId;
            this.OrderItems = orderItems;
        }

        [DataMember]
        public int OrderId { get; init; }

        [DataMember]
        public string? Number { get; init; }

        [DataMember]
        public DateTime Date { get; init; }
            
        [DataMember]
        public int ProviderId { get; init; }

        [DataMember]
        public IEnumerable<OrderItemDto> OrderItems { get; private set; }
    }
}