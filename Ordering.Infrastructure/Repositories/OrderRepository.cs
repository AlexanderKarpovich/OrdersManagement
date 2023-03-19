namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingContext context;

        public OrderRepository(OrderingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => context;

        public Order Add(Order order)
        {
            return context.Orders.Add(order).Entity;
        }

        public void Delete(int orderId)
        {
            Order? order = context.Orders.Find(orderId);

            if (order is not null)
            {
                context.Orders.Remove(order);
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await context.Orders.ToListAsync();
        }

        public async Task<Order?> GetAsync(int orderId)
        {
            var order = await context.Orders.FindAsync(orderId);

            order ??= context.Orders.Local.FirstOrDefault(o => o.Id == orderId);

            if (order is not null)
            {
                await context.Entry(order).Collection(o => o.OrderItems).LoadAsync();
            }

            return order;
        }

        public void Update(Order order)
        {
            context.Entry(order).State = EntityState.Modified;
        }
    }
}