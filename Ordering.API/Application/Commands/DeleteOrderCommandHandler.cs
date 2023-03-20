namespace Ordering.API.Application.Commands
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<CreateOrderCommandHandler> logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("---> Deleting order with the given id: {orderId}", request.OrderId);

            orderRepository.Delete(request.OrderId);
            return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}