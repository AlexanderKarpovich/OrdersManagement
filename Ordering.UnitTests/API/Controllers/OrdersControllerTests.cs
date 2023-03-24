using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.UnitTests.API.Controllers
{
    public class OrdersControllerTests
    {
        private List<Order> orders;
        private Mock<IOrderRepository> repository;
        private Mock<ILogger<OrdersController>> logger;

        public OrdersControllerTests()
        {
            orders = GetDefaultOrders();
            repository = OrderRepositoryMockFactory.Create(orders);

            logger = LoggerMockFactory.CreateLogger<OrdersController>();
        }

        [Fact]
        public async Task CreateOrder_CorrectResponse_ShouldReturnStatus201Created()
        {
            // Arrange
            var command = new CreateOrderCommand("123", DateTime.Now, 123, new List<OrderItemDto>());
            var commandHandler = new CreateOrderCommandHandler(repository.Object, LoggerMockFactory.CreateLogger<CreateOrderCommandHandler>().Object);
            var commandMediator = IMediatorMockFactory.CreateForCommand(commandHandler);
            
            var identifiedHandler = new IdentifiedCommandHandler<CreateOrderCommand, OrderDetailsDto>(commandMediator.Object, IRequestManagerMockFactory.Create<IdentifiedCommand<CreateOrderCommand, OrderDetailsDto>>().Object, LoggerMockFactory.CreateLogger<IdentifiedCommandHandler<CreateOrderCommand, OrderDetailsDto>>().Object);
            var controllerMediator = IMediatorMockFactory.CreateForIdentifiedCommand(identifiedHandler);
            var controller = new OrdersController(controllerMediator.Object, LoggerMockFactory.CreateLogger<OrdersController>().Object);

            // Act
            var response = await controller.CreateOrderAsync(command, Guid.NewGuid());

            // Assert
            Assert.Equal(6, orders.Count);

            var result = Assert.IsType<CreatedAtActionResult>(response);
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
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