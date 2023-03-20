namespace Ordering.API.Application.Commands
{
    public class UpdateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<UpdateOrderCommand, bool>
    {
        public UpdateOrderIdentifiedCommandHandler(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<UpdateOrderCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
