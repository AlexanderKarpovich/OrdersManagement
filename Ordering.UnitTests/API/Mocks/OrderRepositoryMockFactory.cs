namespace Ordering.UnitTests.API.Mocks
{
    public class OrderRepositoryMockFactory
    {
        public static Mock<IOrderRepository> Create(List<Order> orders) 
        {
            var repository = new Mock<IOrderRepository>();

            repository
                .Setup(r => r.UnitOfWork.SaveChangesAsync(default))
                .Returns(Task.FromResult(1));

            repository
                .Setup(r => r.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            repository
                .Setup(r => r.Add(It.IsAny<Order>()))
                .Returns((Order order) => 
                {
                    order.Id = orders.Max(o => o.Id) + 1;
                    orders.Add(order);
                    return order;
                });

            return repository;
        }
    }
}