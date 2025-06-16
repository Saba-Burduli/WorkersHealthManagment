using Domain.Entities;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace Domain.Repositories;

public interface IWorkerTaskRepository
{
    Task<IEnumerable<WorkerTask>> GetAllAsync();
    Task<WorkerTask?> GetByIdAsync(int id);
    Task<IEnumerable<WorkerTask>> GetByWorkerAsync(string workerName);
    Task<IEnumerable<WorkerTask>> GetByStatusAsync(TaskStatus status);
    Task<WorkerTask> CreateAsync(WorkerTask task);
    Task<WorkerTask> UpdateAsync(WorkerTask task);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<WorkerTask>> GetTasksByHealthStatusAsync(WorkerHealthStatus healthStatus);
}