namespace Ordering.API.Application.Queries
{
    public class GetOrderItemsQueryHandler : IRequestHandler<GetOrderItemsQuery, IEnumerable<OrderItemSummary>>
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public GetOrderItemsQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<OrderItemSummary>> Handle(GetOrderItemsQuery request, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.GetDbConnection();
            connection.Open();

            const string orderItemsSQL = 
                @"SELECT oi.[Id], oi.[Name], oi.[Quantity], oi.[Unit] 
                    FROM [ordering].[orderItems] oi
                    WHERE oi.[OrderId]=@id";

            var orderItems = await connection.QueryAsync<OrderItemSummary>(orderItemsSQL, new { id = request.OrderId });

            if (orderItems is null)
            {
                throw new KeyNotFoundException();
            }

            return orderItems;
        }
    }
}