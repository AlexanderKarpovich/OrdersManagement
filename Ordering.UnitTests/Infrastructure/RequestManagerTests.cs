using Ordering.Domain.Exceptions;

namespace Ordering.UnitTests.Infrastructure
{
    public class RequestManagerTests
    {
        private List<ClientRequest> requests;
        private OrderingContext context;

        public RequestManagerTests()
        {
            requests = GetDefaultRequests();
            context = SetupInMemoryContext();
        }

        [Fact]
        public async Task CreateRequest_NewRequest_RequestWithGuidShouldExist()
        {
            // Arrange
            Guid requestId = Guid.NewGuid();
            var requestManager = new RequestManager(context);

            // Act
            await requestManager.CreateRequestForCommandAsync<object>(requestId);

            // Assert
            Assert.NotNull(await context.FindAsync<ClientRequest>(requestId));
        }

        [Fact]
        public async Task CreateRequest_ExistingGuid_ShouldThrowOrderingDomainException()
        {
            // Arrange
            Guid requestId = requests.First().Id;
            var requestManager = new RequestManager(context);

            // Assert
            await Assert.ThrowsAsync<OrderingDomainException>(async () => await requestManager.CreateRequestForCommandAsync<object>(requestId));
        }

        [Fact]
        public async Task Exists_ExistingRequestId_ShouldReturnTrue()
        {
            // Arrange
            Guid requestId = requests.First().Id;
            var requestManager = new RequestManager(context);

            // Assert
            Assert.True(await requestManager.ExistAsync(requestId));
        }

        [Fact]
        public async Task Exists_WrongRequestId_ShouldReturnFalse()
        {
            // Arrange
            Guid requestId = Guid.NewGuid();
            var requestManager = new RequestManager(context);

            // Assert
            Assert.False(await requestManager.ExistAsync(requestId));
        }

        private List<ClientRequest> GetDefaultRequests()
        {
            return new List<ClientRequest>()
            {
                new() { Id = Guid.NewGuid(), Name = "object", Time = DateTime.Now },
                new() { Id = Guid.NewGuid(), Name = "object", Time = DateTime.Now },
                new() { Id = Guid.NewGuid(), Name = "object", Time = DateTime.Now },
                new() { Id = Guid.NewGuid(), Name = "object", Time = DateTime.Now },
                new() { Id = Guid.NewGuid(), Name = "object", Time = DateTime.Now }
            };
        }

        private OrderingContext SetupInMemoryContext()
        {
            string databaseName = Guid.NewGuid().ToString();

            var builder = new DbContextOptionsBuilder<OrderingContext>();
            builder.UseInMemoryDatabase(databaseName);

            var context = new OrderingContext(builder.Options);

            context.AddRange(this.requests);
            context.SaveChanges();

            return context;
        }
    }
}