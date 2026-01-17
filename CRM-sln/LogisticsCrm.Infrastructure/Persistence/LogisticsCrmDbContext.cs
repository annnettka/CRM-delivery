using LogisticsCrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogisticsCrm.Infrastructure.Persistence
{
    public class LogisticsCrmDbContext : DbContext
    {
        public LogisticsCrmDbContext(DbContextOptions<LogisticsCrmDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderStatusHistory> OrderStatusHistory => Set<OrderStatusHistory>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id).HasColumnName("id");

                entity.Property(c => c.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(c => c.ContactPerson)
                    .HasColumnName("contact_person")
                    .HasMaxLength(200);

                entity.Property(c => c.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(50);

                entity.Property(c => c.Email)
                    .HasColumnName("email")
                    .HasMaxLength(200);

                entity.Property(c => c.IsActive)
                    .HasColumnName("is_active");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasKey(o => o.Id);

                entity.Property(o => o.Id).HasColumnName("id");
                entity.Property(o => o.ClientId).HasColumnName("client_id");

                entity.Property(o => o.TrackingNumber)
                    .HasColumnName("tracking_number")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(o => o.TrackingNumber).IsUnique();

                entity.Property(o => o.PickupAddress)
                    .HasColumnName("pickup_address")
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(o => o.DeliveryAddress)
                    .HasColumnName("delivery_address")
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(o => o.RecipientName)
                    .HasColumnName("recipient_name")
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(o => o.RecipientPhone)
                    .HasColumnName("recipient_phone")
                    .HasMaxLength(50);

                entity.Property(o => o.Price)
                    .HasColumnName("price");

                entity.Property(o => o.Status)
                    .HasColumnName("status")
                    .HasConversion<int>();

                entity.Property(o => o.CreatedAtUtc)
                    .HasColumnName("created_at_utc");
            });
            modelBuilder.Entity<OrderStatusHistory>(entity =>
            {
                entity.ToTable("order_status_history");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.OrderId).HasColumnName("order_id");

                entity.Property(x => x.FromStatus)
                    .HasColumnName("from_status")
                    .HasConversion<int>();

                entity.Property(x => x.ToStatus)
                    .HasColumnName("to_status")
                    .HasConversion<int>();

                entity.Property(x => x.ChangedAtUtc)
                    .HasColumnName("changed_at_utc");

                entity.Property(x => x.Comment)
                    .HasColumnName("comment")
                    .HasMaxLength(500);

                entity.HasIndex(x => x.OrderId);
            });

        }
    }
}
