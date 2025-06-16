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


  public class WorkerTaskRepository : IWorkerTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkerTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkerTask>> GetAllAsync()
        {
            return await _context.WorkerTasks.ToListAsync();
        }

        public async Task<WorkerTask?> GetByIdAsync(int id)
        {
            return await _context.WorkerTasks.FindAsync(id);
        }

        public async Task<IEnumerable<WorkerTask>> GetByWorkerAsync(string workerName)
        {
            return await _context.WorkerTasks
                .Where(t => t.AssignedWorker == workerName)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkerTask>> GetByStatusAsync(TaskStatus status)
        {
            return await _context.WorkerTasks
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        public async Task<WorkerTask> CreateAsync(WorkerTask task)
        {
            _context.WorkerTasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<WorkerTask> UpdateAsync(WorkerTask task)
        {
            _context.WorkerTasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.WorkerTasks.FindAsync(id);
            if (task == null) return false;

            _context.WorkerTasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WorkerTask>> GetTasksByHealthStatusAsync(WorkerHealthStatus healthStatus)
        {
            return await _context.WorkerTasks
                .Where(t => t.WorkerHealthStatus == healthStatus)
                .ToListAsync();
        }
    }