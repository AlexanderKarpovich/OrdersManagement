namespace Ordering.API.Application.Commands
{
    [DataContract]
    public class DeleteOrderCommand : IRequest<bool>
    {
        public DeleteOrderCommand(int orderId)
        {
            this.OrderId = orderId;
        }

        [DataMember]
        public int OrderId { get; init; }
    }
}