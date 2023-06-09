namespace Ordering.API.Application.DomainServices
{
    public class OrderNumberUniquenessChecker
    {
        private readonly string connectionString;

        public OrderNumberUniquenessChecker(string connectionString)
        {
            this.connectionString = !string.IsNullOrWhiteSpace(connectionString) ?
                connectionString :
                throw new ArgumentNullException(nameof(connectionString));
        }

        public bool IsUniqueForProvider(string orderNumber, int providerId, int? orderId = null)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            const string sql = 
                @"SELECT TOP 1 [Id]
                    FROM ordering.Orders
                    WHERE Orders.[Number] = @orderNumber 
                    AND Orders.[ProviderId] = @providerId";

            var id = connection.QuerySingleOrDefault<int?>(sql, 
                new { orderNumber, providerId });

            return !id.HasValue || (orderId.HasValue && id == orderId);
        }
    }
}