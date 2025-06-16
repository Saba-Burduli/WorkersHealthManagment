namespace API.BackgroundServices;

public class TaskProcessingWorker
{
     public class TaskProcessingWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TaskProcessingWorker> _logger;

        public TaskProcessingWorker(IServiceProvider serviceProvider, ILogger<TaskProcessingWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task Processing Worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var workerTaskService = scope.ServiceProvider.GetRequiredService<IWorkerTaskService>();

                    // Process pending tasks
                    var pendingTasks = await workerTaskService.GetTasksByStatusAsync("Pending");
                    
                    foreach (var task in pendingTasks.Take(5)) // Process 5 tasks at a time
                    {
                        // Simulate task processing
                        await workerTaskService.UpdateTaskAsync(task.Id, new()
                        {
                            Status = "InProgress",
                            HealthNotes = $"Started processing at {DateTime.UtcNow}"
                        });

                        _logger.LogInformation("Started processing task {TaskId}: {TaskTitle}", task.Id, task.Title);
                    }

                    // Complete long-running tasks (simulate)
                    var inProgressTasks = await workerTaskService.GetTasksByStatusAsync("InProgress");
                    var tasksToComplete = inProgressTasks
                        .Where(t => DateTime.UtcNow.Subtract(t.UpdatedAt ?? t.CreatedAt).TotalMinutes > 10)
                        .Take(2);

                    foreach (var task in tasksToComplete)
                    {
                        await workerTaskService.UpdateTaskAsync(task.Id, new()
                        {
                            Status = "Completed",
                            HealthNotes = $"Completed at {DateTime.UtcNow}"
                        });

                        _logger.LogInformation("Completed task {TaskId}: {TaskTitle}", task.Id, task.Title);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Task Processing Worker");
                }

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}