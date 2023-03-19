namespace Ordering.Infrastructure.EntityConfigurations
{
    public class ClientRequestEntityTypeConfiguration : IEntityTypeConfiguration<ClientRequest>
    {
        public void Configure(EntityTypeBuilder<ClientRequest> builder)
        {
            builder.ToTable("requests", OrderingContext.DefaultSchema);
            builder.HasKey(cr => cr.Id);
            builder.Property(cr => cr.Name).IsRequired(true);
            builder.Property(cr => cr.Time).IsRequired(true);
        }
    }
}