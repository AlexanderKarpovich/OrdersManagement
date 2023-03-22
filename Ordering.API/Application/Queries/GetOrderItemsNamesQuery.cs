namespace Ordering.API.Application.Queries
{
    public class GetOrderItemsNamesQuery : IRequest<IEnumerable<string>>
    {
        public GetOrderItemsNamesQuery(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; init; }
    }
}