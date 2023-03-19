using System.Collections.Generic;

namespace Ordering.UnitTests.Domain
{
    public class OrderTests
    {
        [Fact]
        public void AddOrderItem_OrderItemsShouldContainAddedItem()
        {
            // Arrange
            const string itemName = "Item";
            const int count = 1;

            var order = new Order("1", DateTime.Now, 1);
            var item = new OrderItem(itemName, 1, "Unit");

            // Act
            order.AddOrderItem(item.Name!, item.Quantity, item.Unit!);

            // Assert
            Assert.Equal(count, order.OrderItems.Count);
            Assert.Contains(order.OrderItems, oi => oi.Name == itemName);
        }

        [Fact]
        public void ClearOrderItems_OrderItemsShouldReturnEmptyList()
        {
            // Arrange
            const string itemName = "Item";
            const int count = 0;

            var order = new Order("1", DateTime.Now, 1);
            var item = new OrderItem(itemName, 1, "Unit");
            order.AddOrderItem(item.Name!, item.Quantity, item.Unit!);

            // Act
            order.ClearOrderItems();

            // Assert
            Assert.Equal(count, order.OrderItems.Count);
            Assert.DoesNotContain(order.OrderItems, oi => oi.Name == itemName);
        }

        [Fact]
        public void UpdateOrder_WithoutItems_OrderShouldBeUpdated()
        {
            // Arrange
            var order = new Order("1", DateTime.Now, 1);

            string number = "2";
            DateTime date = DateTime.Today - TimeSpan.FromDays(1);
            int providerId = 2;

            // Act
            order.UpdateOrder(number, date, providerId);

            // Assert
            Assert.Equal(number, order.Number);
            Assert.Equal(date, order.Date);
            Assert.Equal(providerId, order.ProviderId);
        }

        [Fact]
        public void UpdateOrder_WithItems_OrderItemsShouldBeUpdated()
        {
            // Arrange
            var orderItems = new List<OrderItem>()
            {
                new("Item", 1M, "Unit"),
                new("Item", 1M, "Unit"),
                new("Item", 1M, "Unit")
            };

            var order = new Order("1", DateTime.Now, 1);

            foreach (OrderItem item in orderItems)
            {
                order.AddOrderItem(item.Name!, item.Quantity, item.Unit!);
            }

            string number = "2";
            DateTime date = DateTime.Today - TimeSpan.FromDays(1);
            int providerId = 2;

            // Act
            orderItems.RemoveAt(2);
            order.UpdateOrder(number, date, providerId, orderItems);

            // Assert
            Assert.Equal(number, order.Number);
            Assert.Equal(date, order.Date);
            Assert.Equal(providerId, order.ProviderId);
            Assert.Equal(orderItems.Count, order.OrderItems.Count);
        }
    }
}