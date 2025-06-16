using Microsoft.EntityFrameworkCore;

namespace ApplicationDbContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<WorkerTask> WorkerTasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkerTask>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.AssignedWorker).HasMaxLength(100);
            entity.Property(e => e.HealthNotes).HasMaxLength(500);
                
            entity.HasIndex(e => e.AssignedWorker);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.WorkerHealthStatus);
        });
    }
}