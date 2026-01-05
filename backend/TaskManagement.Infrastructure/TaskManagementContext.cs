using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Models;

namespace TaskManagement.Infrastructure;

public class TaskManagementContext : DbContext
{
    public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Telephone)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);
            
            // Unique index on Email
            entity.HasIndex(e => e.Email)
                .IsUnique();
        });

        // TaskItem configuration
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(1000);
            entity.Property(e => e.DueDate)
                .IsRequired();
            entity.Property(e => e.Priority)
                .IsRequired()
                .HasConversion<int>();

            // Foreign key relationship
            entity.HasOne(e => e.User)
                .WithMany(u => u.TaskItems)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
