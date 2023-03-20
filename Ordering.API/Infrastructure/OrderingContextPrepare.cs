namespace Ordering.API.Infrastructure
{
    public class OrderingContextPrepare
    {
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var logger = scope.ServiceProvider
                    .GetRequiredService<ILogger<OrderingContextPrepare>>();

                var policy = CreatePolicy(logger, 5);

                await policy.ExecuteAsync(async () =>
                {
                    using var context = scope.ServiceProvider
                        .GetRequiredService<OrderingContext>();

                    if (context.Database.GetPendingMigrations().Any())
                    {
                        logger.LogInformation("---> Applying database migrations...");

                        context.Database.Migrate();

                        logger.LogInformation("---> Database migrations applied");
                    }

                    if (!context.Providers.Any())
                    {
                        logger.LogInformation("---> No providers found. Seeding data...");

                        await context.Providers.AddRangeAsync(GetDefaultProviders());
                        await context.SaveChangesAsync();

                        logger.LogInformation("---> Providers data seeded");
                    }
                });
            }
        }

        private static IEnumerable<Provider> GetDefaultProviders()
        {
            return new List<Provider>()
            {
                new Provider() { Name = "Novametal" },
                new Provider() { Name = "Strong Steel" },
                new Provider() { Name = "MetalInvest" },
                new Provider() { Name = "Rotinar" },
                new Provider() { Name = "Metall-Trade" }
            };
        }

        private static AsyncRetryPolicy CreatePolicy(ILogger<OrderingContextPrepare> logger, int retries = 3)
        {
            return Policy.Handle<SqlException>()
                .WaitAndRetryAsync(retries,
                    retry => TimeSpan.FromSeconds(3),
                    (exception, timeSpan, retry, context) =>
                    {
                        logger.LogWarning(exception,
                            @$"---> Exception detected on attempt {retry} of 
                                {retries}: {exception.Message}");
                    });
        }
    }
}