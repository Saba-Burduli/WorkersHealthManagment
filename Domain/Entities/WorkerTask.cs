using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("WorkerTask")]
public class WorkerTask
{
    
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string? Title { get; set; }
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public TaskStatus Status { get; set; } = TaskStatus.Pending; 
    
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    
    public string? AssignedWorker { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; }
    
    public DateTime CompletedAt { get; set; }
    
    public WorkerHealthStatus WorkerHealthStatus { get; set; } = WorkerHealthStatus.Healthy;
    
    public int EstimatedHours { get; set; }
    
    public string HealthNotes { get; set; } = string.Empty;
}

public enum TaskStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3,
    OnHold = 4
}
public enum TaskPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

public enum WorkerHealthStatus
{
    Healthy = 0,
    OverLoaded = 1,
    Unavailable = 2,
    Maintenance = 3
}
    
