namespace Ordering.Infrastructure.EntityConfigurations
{
    public class ProviderEntityTypeConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.ToTable("providers", OrderingContext.DefaultSchema);

            builder.HasKey(p => p.Id);
            builder.Ignore(p => p.DomainEvents);

            builder
                .Property(p => p.Id)
                .UseHiLo("providerseq", OrderingContext.DefaultSchema)
                .IsRequired(true);

            builder
                .Property(p => p.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(max)")
                .IsRequired(true);
        }
    }
}