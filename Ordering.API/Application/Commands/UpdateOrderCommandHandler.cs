namespace Ordering.API.Application.Commands
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, bool>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<CreateOrderCommandHandler> logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetAsync(request.OrderId);

            if (order is not null)
            {
                order.UpdateOrder(request.Number!, request.Date, request.ProviderId);
                order.ClearOrderItems();

                foreach (OrderItemDto item in request.OrderItems)
                {
                    order.AddOrderItem(item.Name!, item.Quantity, item.Unit!);
                }

                logger.LogInformation($"---> Updating order: {order}");

                orderRepository.Update(order);
                return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);              
            }

            return false;
        }
    }
}