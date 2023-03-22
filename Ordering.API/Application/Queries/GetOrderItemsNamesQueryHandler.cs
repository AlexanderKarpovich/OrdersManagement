namespace Ordering.API.Application.Queries
{
    public class GetOrderItemsNamesQueryHandler : IRequestHandler<GetOrderItemsNamesQuery, IEnumerable<string>>
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public GetOrderItemsNamesQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<string>> Handle(GetOrderItemsNamesQuery request, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.GetDbConnection();
            connection.Open();

            const string orderItemsSQL = 
                @"SELECT DISTINCT oi.[Name] 
                    FROM [ordering].[orderItems] oi
                    WHERE oi.[OrderId]=@id";

            var orderItemsNames = await connection.QueryAsync<string>(orderItemsSQL, new { id = request.OrderId });

            if (orderItemsNames is null)
            {
                throw new KeyNotFoundException();
            }

            return orderItemsNames;
        }
    }
}