namespace Ordering.API.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDetailsDto>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<CreateOrderCommandHandler> logger;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OrderDetailsDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(request.Number!, request.Date, request.ProviderId);

            foreach (OrderItemDto item in request.OrderItems)
            {
                order.AddOrderItem(item.Name!, item.Quantity, item.Unit!);
            }

            logger.LogInformation("---> Creating Order: {order}", order);

            orderRepository.Add(order);
            await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return OrderDetailsDto.FromOrder(order);
        }
    }
}