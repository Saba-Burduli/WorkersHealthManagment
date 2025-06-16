namespace API.BackgroundServices;

public class HealthMonitoringWorker
{
     public class HealthMonitoringWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HealthMonitoringWorker> _logger;

        public HealthMonitoringWorker(IServiceProvider serviceProvider, ILogger<HealthMonitoringWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Health Monitoring Worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var workerTaskService = scope.ServiceProvider.GetRequiredService<IWorkerTaskService>();

                    var allTasks = await workerTaskService.GetAllTasksAsync();
                    var workerGroups = allTasks.GroupBy(t => t.AssignedWorker);

                    foreach (var workerGroup in workerGroups)
                    {
                        var workerName = workerGroup.Key;
                        var activeTasks = workerGroup.Count(t => t.Status == "InProgress" || t.Status == "Pending");
                        
                        var healthStatus = activeTasks switch
                        {
                            > 10 => "Overloaded",
                            > 5 => "Healthy",
                            _ => "Healthy"
                        };

                        foreach (var task in workerGroup.Where(t => t.WorkerHealthStatus != healthStatus))
                        {
                            await workerTaskService.UpdateTaskAsync(task.Id, new()
                            {
                                WorkerHealthStatus = healthStatus,
                                HealthNotes = $"Auto-updated by health monitor at {DateTime.UtcNow}"
                            });
                        }
                    }

                    _logger.LogInformation("Health monitoring completed for {WorkerCount} workers", workerGroups.Count());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Health Monitoring Worker");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}

