namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        private string? number;
        private DateTime date;
        private int providerId;
        private readonly List<OrderItem> orderItems;

        public string? Number => number;
        public DateTime Date => date;
        public int ProviderId => providerId;

        public IReadOnlyCollection<OrderItem> OrderItems => orderItems;

        protected Order()
        {
            orderItems = new List<OrderItem>();
        }

        public Order(string number, DateTime date, int providerId) : this()
        {
            this.number = number;
            this.date = date;
            this.providerId = providerId;
        }

        public void UpdateOrder(string number, DateTime date, int providerId, IEnumerable<OrderItem>? items = null)
        {
            this.number = number;
            this.date = date;
            this.providerId = providerId;

            if (items is not null)
            {
                orderItems.Clear();
                foreach (OrderItem item in items)
                {
                    orderItems.Add(item);
                }
            }
        }

        public void ClearOrderItems()
        {
            orderItems.Clear();
        }

        public void AddOrderItem(string name, decimal quantity, string unit)
        {
            var orderItem = new OrderItem(name, quantity, unit);
            orderItems.Add(orderItem);
        }
    }
}
