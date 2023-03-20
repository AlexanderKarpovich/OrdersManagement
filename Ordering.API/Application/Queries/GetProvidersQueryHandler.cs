namespace Ordering.API.Application.Queries
{
    public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, IEnumerable<ProviderSummary>>
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public GetProvidersQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<ProviderSummary>> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.GetDbConnection();
            connection.Open();

            const string sql = 
                @"SELECT p.[Id], p.[Name] 
                    FROM [ordering].[providers] p";

            return await connection.QueryAsync<ProviderSummary>(sql);
        }
    }
}