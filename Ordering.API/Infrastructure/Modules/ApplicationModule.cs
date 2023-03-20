namespace Ordering.API.Infrastructure.Modules
{
    public class ApplicationModule : Autofac.Module
    {
        private readonly string connectionString;

        public ApplicationModule(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Register the service responsible for checking order number uniqueness
            builder.Register(c => new OrderNumberUniquenessChecker(connectionString)).AsSelf()
                .InstancePerLifetimeScope();

            // Register request manager
            builder.RegisterType<RequestManager>()
                .As<IRequestManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .InstancePerLifetimeScope();

            builder.Register(c => new SqlConnectionFactory(connectionString))
                .As<ISqlConnectionFactory>()
                .InstancePerLifetimeScope();
        }
    }
}