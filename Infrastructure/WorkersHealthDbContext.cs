using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

    public class WorkersHealthDbContext : DbContext
    {
        public WorkersHealthDbContext(DbContextOptions<WorkersHealthDbContext> options) : base(options)
        {
           
        }

        public DbSet<WorkerTask> WorkerTasks { get; set; }

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