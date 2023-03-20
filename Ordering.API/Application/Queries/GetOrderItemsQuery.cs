namespace Ordering.API.Application.Queries
{
    public class GetOrderItemsQuery : IRequest<IEnumerable<OrderItemSummary>>
    {
        public GetOrderItemsQuery(int orderId)
        {
            this.OrderId = orderId;
        }

        public int OrderId { get; init; }
    }
}