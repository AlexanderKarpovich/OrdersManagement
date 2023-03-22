namespace Ordering.API.Application.Queries
{
    public class GetOrderItemsUnitsQueryHandler : IRequestHandler<GetOrderItemsUnitsQuery, IEnumerable<string>>
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public GetOrderItemsUnitsQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<string>> Handle(GetOrderItemsUnitsQuery request, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.GetDbConnection();
            connection.Open();

            const string orderItemsSQL = 
                @"SELECT DISTINCT oi.[Unit] 
                    FROM [ordering].[orderItems] oi
                    WHERE oi.[OrderId]=@id";

            var orderItemsUnits = await connection.QueryAsync<string>(orderItemsSQL, new { id = request.OrderId });

            if (orderItemsUnits is null)
            {
                throw new KeyNotFoundException();
            }

            return orderItemsUnits;
        }
    }
}