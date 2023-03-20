namespace Ordering.API.Application.Queries
{
    public class GetOrderQuery : IRequest<OrderDetails>
    {
        public int Id { get; }

        public GetOrderQuery(int id)
        {
            this.Id = id;
        }
    }
}