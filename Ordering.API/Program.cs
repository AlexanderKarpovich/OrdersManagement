var builder = WebApplication.CreateBuilder(args);

// Configuring DI service collection
builder.Services
    .AddCustomControllers()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCustomHealthChecks(builder.Configuration)
    .AddCustomDbContext(builder.Configuration)
    .AddCustomConfiguration();

// Configure Autofac as the service provider
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterMediatR(typeof(Program).Assembly);
        containerBuilder.RegisterModule(new MediatorModule());
        containerBuilder.RegisterModule(new ApplicationModule(
            builder.Configuration.GetConnectionString("OrderingConnection") ?? ""));
    });

// Configuring Serilog logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("../Logs/api-log-.txt", rollingInterval: RollingInterval.Hour)
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Using Serilog as logging provider
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/liveness", new HealthCheckOptions()
{
    Predicate = r => r.Name.Contains("self")
});

await OrderingContextPrepare.EnsurePopulated(app);

app.Run();

