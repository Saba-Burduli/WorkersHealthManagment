using Application.DTOs;

namespace Application.Interfaces;

public interface IWorkerTaskService
{
    Task<IEnumerable<WorkerTaskDto>> GetAllTasksAsync();
    Task<WorkerTaskDto?> GetTaskByIdAsync(int id);
    Task<IEnumerable<WorkerTaskDto>> GetTasksByWorkerAsync(string workerName);
    Task<IEnumerable<WorkerTaskDto>> GetTasksByStatusAsync(string status);
    Task<WorkerTaskDto> CreateTaskAsync(CreateWorkerTaskDto createDto);
    Task<WorkerTaskDto> UpdateTaskAsync(int id, UpdateWorkerTaskDto updateDto);
    Task<bool> DeleteTaskAsync(int id);
    Task<IEnumerable<WorkerTaskDto>> GetTasksByHealthStatusAsync(string healthStatus);
    Task<Dictionary<string, int>> GetWorkerHealthSummaryAsync();
}