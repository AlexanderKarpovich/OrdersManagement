namespace Ordering.API.Application.Queries
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderSummary>>
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public GetOrdersQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<OrderSummary>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.GetDbConnection();
            connection.Open();

            const string sql = 
                @"SELECT o.[Id], o.[Number], o.[Date], o.[ProviderId] 
                    FROM [ordering].[orders] o";

            return await connection.QueryAsync<OrderSummary>(sql);
        }
    }
}