namespace Ordering.Infrastructure.EntityConfigurations
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("orderItems", OrderingContext.DefaultSchema);

            builder.HasKey(oi => oi.Id);
            builder.Ignore(oi => oi.DomainEvents);

            builder
                .Property(oi => oi.Id)
                .UseHiLo("orderitemseq", OrderingContext.DefaultSchema)
                .IsRequired(true);

            builder
                .Property<string?>("name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(max)")
                .IsRequired(true)
                .IsConcurrencyToken(true);

            builder
                .Property<decimal>("quantity")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Quantity")
                .HasColumnType("decimal(18,3)")
                .HasPrecision(18, 3)
                .IsRequired(true)
                .IsConcurrencyToken(true);

            builder
                .Property<string?>("unit")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Unit")
                .HasColumnType("nvarchar(max)")
                .IsRequired(true)
                .IsConcurrencyToken(true);

            builder
                .Property<int>("orderId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("OrderId")
                .HasColumnType("int")
                .IsRequired(true);
        }
    }
}