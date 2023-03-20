namespace Ordering.API.Application.Commands
{
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
        where T : IRequest<R>
    {
        private readonly IMediator mediator;
        private readonly IRequestManager requestManager;
        private readonly ILogger<IdentifiedCommandHandler<T, R>> logger;

        public IdentifiedCommandHandler(IMediator mediator, 
            IRequestManager requestManager, 
            ILogger<IdentifiedCommandHandler<T, R>> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<R> Handle(IdentifiedCommand<T, R> request, CancellationToken cancellationToken)
        {
            var exists = await requestManager.ExistAsync(request.Id);
            if (exists)
            {
                return CreateResultForDuplicateRequest();
            }

            await requestManager.CreateRequestForCommandAsync<T>(request.Id);
            try
            {
                var command = request.Command;
                var commandName = command.GetType().Name;

                logger.LogInformation("---> Sending command {commandName} ({command})",
                    commandName, command);

                var result = await mediator.Send(command, cancellationToken);

                logger.LogInformation("---> Command successfully sent. Result: {result}", result);

                return result;
            }
            catch 
            {
                return default!;
            }
        }

        protected virtual R CreateResultForDuplicateRequest()
        {
            return default!;
        }
    }
}