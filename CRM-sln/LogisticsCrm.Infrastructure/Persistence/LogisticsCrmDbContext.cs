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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id)
                    .HasColumnName("id");

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
        }
    }
}
