namespace Application.DTOs;

public class CreateWorkerTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public string AssignedWorker { get; set; } = string.Empty;
    public int EstimatedHours { get; set; }
    public string HealthNotes { get; set; } = string.Empty;
}