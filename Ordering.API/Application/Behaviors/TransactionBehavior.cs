namespace Ordering.API.Application.Behaviors
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> logger;
        private readonly OrderingContext context;

        public TransactionBehaviour(OrderingContext context,
            ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = default(TResponse);
            var typeName = request.GetType().Name;

            try
            {
                if (context.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = context.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await context.BeginTransactionAsync();

                    using (LogContext.PushProperty("TransactionContext", transaction?.TransactionId))
                    {
                        logger.LogInformation("---> Begin transaction {transactionId} for {typeName} ({request})",
                            transaction?.TransactionId, typeName, request);

                        response = await next();

                        logger.LogInformation("---> Commit transaction {transactionId} for {typeName}",
                            transaction?.TransactionId, typeName);

                        if (transaction is not null)
                        {
                            await context.CommitTransactionAsync(transaction);
                        }
                    }
                });

                return response!;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error handling transaction for {typeName} ({request})", typeName, request);
                throw;
            }
        }
    }
}