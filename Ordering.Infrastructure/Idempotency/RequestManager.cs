namespace Ordering.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly OrderingContext context;

        public RequestManager(OrderingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ? 
                throw new OrderingDomainException($"Request with {id} already exists") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            context.Add(request);

            await context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await context.FindAsync<ClientRequest>(id);

            return request is not null;
        }
    }
}