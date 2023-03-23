namespace Ordering.UnitTests.Infrastructure
{
    public class OrderRepositoryTests
    {
        private List<Order> orders;
        private OrderingContext context;

        public OrderRepositoryTests()
        {
            orders = GetDefaultOrders();
            context = SetupInMemoryContext();
        }

        [Fact]
        public async Task GetAllOrders_OrdersCountShouldReturnFive()
        {
            // Arrange
            const int expectedCount = 5;
            var repository = new OrderRepository(context);

            // Act
            var orders = await repository.GetAllAsync();

            // Assert
            Assert.Equal(expectedCount, orders.Count());
            Assert.Equal(orders, this.orders);
        }

        [Fact]
        public async Task GetOrder_CorrectId_ShouldReturnOrder()
        {
            // Arrange
            const int orderId = 1;
            Order? expectedOrder = orders.Find(o => o.Id == orderId);
            var repository = new OrderRepository(context);

            // Act
            var order = await repository.GetAsync(orderId);

            // Assert
            Assert.NotNull(order);
            Assert.Equal(expectedOrder, order);
        }

        [Fact]
        public async Task GetOrder_WrongId_ShouldReturnNull()
        {
            // Arrange
            const int orderId = -1;
            var repository = new OrderRepository(context);

            // Act
            var order = await repository.GetAsync(orderId);

            // Assert
            Assert.Null(order);
        }

        [Fact]
        public async Task AddNewOrder_ContextShouldContainNewOrder()
        {
            // Arrange
            Order newOrder = new("6", DateTime.Now, 6) { Id = 6 };
            var repository = new OrderRepository(context);

            // Act
            var order = repository.Add(newOrder);

            // Assert
            Assert.True(await repository.UnitOfWork.SaveEntitiesAsync());
            Assert.Contains(newOrder, context.Orders);
        }

        [Fact]
        public async Task DeleteOrder_CorrectId_ContextShouldNotContainDeletedOrder()
        {
            // Arrange
            const int orderId = 1;
            var removedOrder = orders.Find(o => o.Id == orderId);
            var repository = new OrderRepository(context);

            // Act
            repository.Delete(orderId);
            await repository.UnitOfWork.SaveChangesAsync();

            // Assert
            Assert.DoesNotContain(removedOrder, context.Orders);
        }

        [Fact]
        public async Task DeleteOrder_WrongId_ContextShouldBeTheSame()
        {
            // Arrange
            const int orderId = -1;
            var repository = new OrderRepository(context);

            // Act
            repository.Delete(orderId);
            await repository.UnitOfWork.SaveChangesAsync();

            // Assert
            Assert.Equal(orders, context.Orders);
        }

        [Fact]
        public async Task UpdateOrder_CorrectOrder_OrderShouldBeUpdated()
        {
            // Arrange
            var updatedOrder = orders.First();
            updatedOrder.UpdateOrder("123", DateTime.Now, 123);
            var repository = new OrderRepository(context);

            // Act
            repository.Update(updatedOrder);

            // Assert
            Assert.True(await repository.UnitOfWork.SaveEntitiesAsync());
            Assert.Contains(updatedOrder, context.Orders);
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

        private OrderingContext SetupInMemoryContext()
        {
            string databaseName = Guid.NewGuid().ToString();

            var builder = new DbContextOptionsBuilder<OrderingContext>();
            builder.UseInMemoryDatabase(databaseName);

            var context = new OrderingContext(builder.Options);

            context.AddRange(this.orders);
            context.SaveChanges();

            return context;
        }
    }
}