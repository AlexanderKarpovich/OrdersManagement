namespace Ordering.API.Infrastructure.DataServices
{
    public interface ISqlConnectionFactory
    {
        public IDbConnection GetDbConnection();
    }
}