
namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasConversion(dbWriteId => dbWriteId.Value, dbReadId => OrderId.Of(dbReadId));
        builder.HasOne<Customer>().WithMany().HasForeignKey(o => o.CustomerId).IsRequired();
        //builder.HasMany<OrderItem>().WithOne().HasForeignKey(oi => oi.OrderId);
        builder.HasMany(o => o.OrderItems).WithOne(oi => oi.Order).HasForeignKey(OrderItem => OrderItem.OrderId);

        builder.ComplexProperty(o => o.OrderName, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value).HasColumnName(nameof(Order.OrderName)).IsRequired().HasMaxLength(100);
        });
        
        builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
            addressBuilder.Property(a => a.LastName).IsRequired().HasMaxLength(50);
            addressBuilder.Property(a => a.EmailAddress).IsRequired().HasMaxLength(50);
            addressBuilder.Property(a => a.AddressLine).IsRequired().HasMaxLength(180);
            addressBuilder.Property(a => a.Country).HasMaxLength(50);
            addressBuilder.Property(a => a.State).HasMaxLength(50);
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(5);
        });
        
        builder.ComplexProperty(o => o.BillingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
            addressBuilder.Property(a => a.LastName).IsRequired().HasMaxLength(50);
            addressBuilder.Property(a => a.EmailAddress).IsRequired().HasMaxLength(50);
            addressBuilder.Property(a => a.AddressLine).IsRequired().HasMaxLength(180);
            addressBuilder.Property(a => a.Country).HasMaxLength(50);
            addressBuilder.Property(a => a.State).HasMaxLength(50);
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(5);
        });
        
        builder.ComplexProperty(o => o.Payment, paymentBuilder =>
        {
            paymentBuilder.Property(p => p.PaymentMethod);
            paymentBuilder.Property(p => p.CardName).HasMaxLength(50);
            paymentBuilder.Property(p => p.CardNumber).HasMaxLength(24).IsRequired();
            paymentBuilder.Property(p => p.Expiration).HasMaxLength(10);
            paymentBuilder.Property(p => p.CVV).HasMaxLength(3);

        });
        
        builder.Property(o => o.Status).HasDefaultValue(OrderStatus.Draft)
            .HasConversion(dbWrite => dbWrite.ToString(), DbDataReader => Enum.Parse<OrderStatus>(DbDataReader));

        builder.Property(o => o.TotalPrice).HasPrecision(18,2);
    }
}
