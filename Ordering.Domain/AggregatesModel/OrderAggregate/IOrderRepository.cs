namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order Add(Order order);
        void Update(Order order);
        void Delete(int orderId);
        Task<Order?> GetAsync(int orderId);
        Task<IEnumerable<Order>> GetAllAsync();
    }
}