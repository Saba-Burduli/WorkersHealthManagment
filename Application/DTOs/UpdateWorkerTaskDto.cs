namespace Application.DTOs;

public class UpdateWorkerTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? AssignedWorker { get; set; }
    public string? WorkerHealthStatus { get; set; }
    public int? EstimatedHours { get; set; }
    public string? HealthNotes { get; set; }
}