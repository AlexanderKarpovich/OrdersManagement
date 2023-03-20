namespace Ordering.API.Application.Queries
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDetails>
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public GetOrderQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<OrderDetails> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.GetDbConnection();
            connection.Open();

            var orderSummary = await GetOrder(connection, request.Id);
            var orderItemsSummary = await GetOrderItems(connection, request.Id);

            if (orderSummary is null || orderItemsSummary is null)
            {
                throw new KeyNotFoundException();
            }

            return new OrderDetails(orderSummary, orderItemsSummary);
        }

        private async Task<OrderSummary> GetOrder(IDbConnection connection, int orderId)
        {
            const string orderSQL = 
                @"SELECT o.[Id], o.[Number], o.[Date], o.[ProviderId] 
                    FROM [ordering].[orders] o 
                    WHERE o.[Id]=@id";

            return await connection.QueryFirstOrDefaultAsync<OrderSummary>(orderSQL, new { id = orderId });
        }

        private async Task<IEnumerable<OrderItemSummary>> GetOrderItems(IDbConnection connection, int orderId)
        {
            const string orderItemsSQL = 
                @"SELECT oi.[Id], oi.[Name], oi.[Quantity], oi.[Unit] 
                    FROM [ordering].[orderItems] oi
                    WHERE oi.[OrderId]=@id";

            return await connection.QueryAsync<OrderItemSummary>(orderItemsSQL, new { id = orderId });
        }
    }
}