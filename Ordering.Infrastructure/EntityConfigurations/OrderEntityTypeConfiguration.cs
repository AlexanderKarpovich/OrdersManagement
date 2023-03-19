namespace Ordering.Infrastructure.EntityConfigurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders", OrderingContext.DefaultSchema);

            builder.HasKey(o => o.Id);
            builder.Ignore(o => o.DomainEvents);
            
            builder
                .Property(o => o.Id)
                .UseHiLo("orderseq", OrderingContext.DefaultSchema)
                .IsRequired(true);

            builder
                .Property<string?>("number")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Number")
                .HasColumnType("nvarchar(max)")
                .IsRequired(true);

            builder
                .Property<DateTime>("date")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Date")
                .HasColumnType("datetime2(7)")
                .IsRequired(true);

            builder
                .Property<int>("providerId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ProviderId")
                .HasColumnType("int")
                .IsRequired(true);

            var itemsNavigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
            itemsNavigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder
                .HasOne<Provider>()
                .WithMany()
                .HasForeignKey("providerId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
        }
    }
}