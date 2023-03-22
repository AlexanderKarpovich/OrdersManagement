namespace Ordering.API.Application.Queries
{
    public class GetOrderItemsUnitsQuery : IRequest<IEnumerable<string>>
    {
        public GetOrderItemsUnitsQuery(int orderId)
        {
            this.OrderId = orderId;
        }

        public int OrderId { get; init; }
    }
}