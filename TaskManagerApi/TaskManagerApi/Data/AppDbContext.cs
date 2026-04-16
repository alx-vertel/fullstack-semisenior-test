using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

namespace TaskManagerApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.Property(t => t.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(t => new { t.UserId, t.Status, t.CreatedAt })
                    .HasDatabaseName("IX_Tasks_User_Status_Date");

                entity.ToTable(t => t.HasCheckConstraint("CHK_Tasks_JSON", "ISJSON(AdditionalInfo) > 0"));
            });
        }
    }
}
