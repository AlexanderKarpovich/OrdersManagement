namespace Ordering.API.Application.Commands
{
    public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateOrderCommand, OrderDetailsDto>
    {
        public CreateOrderIdentifiedCommandHandler(IMediator mediator, 
            IRequestManager requestManager, 
            ILogger<IdentifiedCommandHandler<CreateOrderCommand, OrderDetailsDto>> logger) 
            : base(mediator, requestManager, logger)
        {
        }

        protected override OrderDetailsDto CreateResultForDuplicateRequest()
        {
            return new OrderDetailsDto();
        }
    }
}
