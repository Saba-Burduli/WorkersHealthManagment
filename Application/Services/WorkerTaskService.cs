
namespace Application.Services;

public class WorkerTaskService
{
      public class WorkerTaskService : IWorkerTaskService
    {
        private readonly IWorkerTaskRepository _repository;

        public WorkerTaskService(IWorkerTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<WorkerTaskDto>> GetAllTasksAsync()
        {
            var tasks = await _repository.GetAllAsync();
            return tasks.Select(MapToDto);
        }

        public async Task<WorkerTaskDto?> GetTaskByIdAsync(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            return task != null ? MapToDto(task) : null;
        }

        public async Task<IEnumerable<WorkerTaskDto>> GetTasksByWorkerAsync(string workerName)
        {
            var tasks = await _repository.GetByWorkerAsync(workerName);
            return tasks.Select(MapToDto);
        }

        public async Task<IEnumerable<WorkerTaskDto>> GetTasksByStatusAsync(string status)
        {
            if (Enum.TryParse<TaskStatus>(status, true, out var taskStatus))
            {
                var tasks = await _repository.GetByStatusAsync(taskStatus);
                return tasks.Select(MapToDto);
            }
            return Enumerable.Empty<WorkerTaskDto>();
        }

        public async Task<WorkerTaskDto> CreateTaskAsync(CreateWorkerTaskDto createDto)
        {
            var task = new WorkerTask
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Priority = Enum.TryParse<TaskPriority>(createDto.Priority, true, out var priority) ? priority : TaskPriority.Medium,
                AssignedWorker = createDto.AssignedWorker,
                EstimatedHours = createDto.EstimatedHours,
                HealthNotes = createDto.HealthNotes,
                CreatedAt = DateTime.UtcNow
            };

            var createdTask = await _repository.CreateAsync(task);
            return MapToDto(createdTask);
        }

        public async Task<WorkerTaskDto> UpdateTaskAsync(int id, UpdateWorkerTaskDto updateDto)
        {
            var existingTask = await _repository.GetByIdAsync(id);
            if (existingTask == null)
                throw new KeyNotFoundException($"Task with ID {id} not found");

            if (!string.IsNullOrEmpty(updateDto.Title))
                existingTask.Title = updateDto.Title;
            
            if (!string.IsNullOrEmpty(updateDto.Description))
                existingTask.Description = updateDto.Description;
            
            if (!string.IsNullOrEmpty(updateDto.Status) && Enum.TryParse<TaskStatus>(updateDto.Status, true, out var status))
            {
                existingTask.Status = status;
                if (status == TaskStatus.Completed)
                    existingTask.CompletedAt = DateTime.UtcNow;
            }
            
            if (!string.IsNullOrEmpty(updateDto.Priority) && Enum.TryParse<TaskPriority>(updateDto.Priority, true, out var priority))
                existingTask.Priority = priority;
            
            if (!string.IsNullOrEmpty(updateDto.AssignedWorker))
                existingTask.AssignedWorker = updateDto.AssignedWorker;
            
            if (!string.IsNullOrEmpty(updateDto.WorkerHealthStatus) && Enum.TryParse<WorkerHealthStatus>(updateDto.WorkerHealthStatus, true, out var healthStatus))
                existingTask.WorkerHealthStatus = healthStatus;
            
            if (updateDto.EstimatedHours.HasValue)
                existingTask.EstimatedHours = updateDto.EstimatedHours.Value;
            
            if (!string.IsNullOrEmpty(updateDto.HealthNotes))
                existingTask.HealthNotes = updateDto.HealthNotes;

            existingTask.UpdatedAt = DateTime.UtcNow;

            var updatedTask = await _repository.UpdateAsync(existingTask);
            return MapToDto(updatedTask);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkerTaskDto>> GetTasksByHealthStatusAsync(string healthStatus)
        {
            if (Enum.TryParse<WorkerHealthStatus>(healthStatus, true, out var workerHealthStatus))
            {
                var tasks = await _repository.GetTasksByHealthStatusAsync(workerHealthStatus);
                return tasks.Select(MapToDto);
            }
            return Enumerable.Empty<WorkerTaskDto>();
        }

        public async Task<Dictionary<string, int>> GetWorkerHealthSummaryAsync()
        {
            var allTasks = await _repository.GetAllAsync();
            return allTasks
                .GroupBy(t => t.WorkerHealthStatus.ToString())
                .ToDictionary(g => g.Key, g => g.Count());
        }

        private static WorkerTaskDto MapToDto(WorkerTask task)
        {
            return new WorkerTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                AssignedWorker = task.AssignedWorker,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                CompletedAt = task.CompletedAt,
                WorkerHealthStatus = task.WorkerHealthStatus.ToString(),
                EstimatedHours = task.EstimatedHours,
                HealthNotes = task.HealthNotes
            };
        }
    }
}