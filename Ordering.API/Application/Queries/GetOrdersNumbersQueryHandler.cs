namespace Ordering.API.Application.Queries
{
    public class GetOrdersNumbersQueryHandler : IRequestHandler<GetOrdersNumbersQuery, IEnumerable<string>>
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public GetOrdersNumbersQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<string>> Handle(GetOrdersNumbersQuery request, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.GetDbConnection();
            connection.Open();

            const string sql = 
                @"SELECT DISTINCT o.[Number] 
                    FROM [ordering].[orders] o";

            return await connection.QueryAsync<string>(sql);
        }
    }
}