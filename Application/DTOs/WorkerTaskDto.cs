namespace Application.DTOs;

public class WorkerTaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string AssignedWorker { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string WorkerHealthStatus { get; set; } = string.Empty;
    public int EstimatedHours { get; set; }
    public string HealthNotes { get; set; } = string.Empty;
}