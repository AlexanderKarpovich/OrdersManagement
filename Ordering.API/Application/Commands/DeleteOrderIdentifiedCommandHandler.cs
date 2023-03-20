namespace Ordering.API.Application.Commands
{
    public class DeleteOrderIdentifiedCommandHandler : IdentifiedCommandHandler<DeleteOrderCommand, bool>
    {
        public DeleteOrderIdentifiedCommandHandler(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<DeleteOrderCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
