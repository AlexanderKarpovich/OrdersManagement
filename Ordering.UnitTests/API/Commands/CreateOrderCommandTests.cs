namespace Ordering.UnitTests.API
{
    public class CreateOrderCommandTests
    {
        private List<Order> orders;
        private Mock<IOrderRepository> repository;
        private Mock<ILogger<CreateOrderCommandHandler>> logger;

        public CreateOrderCommandTests()
        {
            orders = GetDefaultOrders();
            repository = OrderRepositoryMockFactory.Create(orders);
            logger = GenericLoggerMockFactory.CreateLogger<CreateOrderCommandHandler>();
        }

        [Fact]
        public async Task HandleCreateCommand_OrderShouldBeAdded()
        {
            // Arrange
            var request = new CreateOrderCommand("123", DateTime.Now, 123, new List<OrderItemDto>());
            var handler = new CreateOrderCommandHandler(repository.Object, logger.Object);

            // Act
            await handler.Handle(request, default);

            // Assert
            repository.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
            repository.Verify(r => r.UnitOfWork.SaveEntitiesAsync(default), Times.Once);

            Assert.Equal(6, orders.Count);
        }

        private List<Order> GetDefaultOrders()
        {
            return new List<Order>()
            {
                new("1", DateTime.Now, 1) { Id = 1 },
                new("2", DateTime.Now, 2) { Id = 2 },
                new("3", DateTime.Now, 3) { Id = 3 },
                new("4", DateTime.Now, 4) { Id = 4 },
                new("5", DateTime.Now, 5) { Id = 5 }
            };
        }
    }
}