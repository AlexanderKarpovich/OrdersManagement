namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class OrderItem : Entity
    {
        private string? name;
        private decimal quantity;
        private string? unit;
        private int orderId;

        public OrderItem() { }

        public OrderItem(string name, decimal quantity, string unit)
        {
            this.name = name;
            this.quantity = quantity;
            this.unit = unit;
            orderId = 0;
        }

        public string? Name => name;
        
        public decimal Quantity => quantity;

        public string? Unit => unit;

        public int OrderId => orderId;
    }
}
