HEADERnamespace a;

public class WorkloadBalancingWorker
{
     public class WorkloadBalancingWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WorkloadBalancingWorker> _logger;
        private readonly string[] _availableWorkers = { "Worker1", "Worker2", "Worker3", "Worker4", "Worker5" };

        public WorkloadBalancingWorker(IServiceProvider serviceProvider, ILogger<WorkloadBalancingWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Workload Balancing Worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var workerTaskService = scope.ServiceProvider.GetRequiredService<IWorkerTaskService>();

                    // Find unassigned tasks
                    var allTasks = await workerTaskService.GetAllTasksAsync();
                    var unassignedTasks = allTasks.Where(t => string.IsNullOrEmpty(t.AssignedWorker) && t.Status == "Pending").ToList();

                    if (unassignedTasks.Any())
                    {
                        // Get current workload for each worker
                        var workerWorkloads = new Dictionary<string, int>();
                        foreach (var worker in _availableWorkers)
                        {
                            var workerTasks = await workerTaskService.GetTasksByWorkerAsync(worker);
                            workerWorkloads[worker] = workerTasks.Count(t => t.Status == "InProgress" || t.Status == "Pending");
                        }

                        // Assign tasks to workers with least workload
                        foreach (var task in unassignedTasks.Take(10))
                        {
                            var assignedWorker = workerWorkloads.OrderBy(w => w.Value).First().Key;
                            
                            await workerTaskService.UpdateTaskAsync(task.Id, new()
                            {
                                AssignedWorker = assignedWorker,
                                HealthNotes = $"Auto-assigned to {assignedWorker} at {DateTime.UtcNow}"
                            });

                            workerWorkloads[assignedWorker]++;
                            _logger.LogInformation("Assigned task {TaskId} to worker {WorkerName}", task.Id, assignedWorker);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Workload Balancing Worker");
                }

                await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            }
        }
    }
}