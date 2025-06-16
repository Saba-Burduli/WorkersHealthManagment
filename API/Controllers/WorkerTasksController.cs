// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
//
// namespace API.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class WorkerTasksController : ControllerBase
//     {
//           [ApiController]
//     [Route("api/[controller]")]
//     public class WorkerTasksController : ControllerBase
//     {
//         private readonly IWorkerTaskService _workerTaskService;
//         private readonly ILogger<WorkerTasksController> _logger;
//
//         public WorkerTasksController(IWorkerTaskService workerTaskService, ILogger<WorkerTasksController> logger)
//         {
//             _workerTaskService = workerTaskService;
//             _logger = logger;
//         }
//
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<WorkerTaskDto>>> GetAllTasks()
//         {
//             try
//             {
//                 var tasks = await _workerTaskService.GetAllTasksAsync();
//                 return Ok(tasks);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error retrieving all tasks");
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//
//         [HttpGet("{id}")]
//         public async Task<ActionResult<WorkerTaskDto>> GetTask(int id)
//         {
//             try
//             {
//                 var task = await _workerTaskService.GetTaskByIdAsync(id);
//                 if (task == null)
//                     return NotFound($"Task with ID {id} not found");
//
//                 return Ok(task);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error retrieving task {TaskId}", id);
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//
//         [HttpGet("worker/{workerName}")]
//         public async Task<ActionResult<IEnumerable<WorkerTaskDto>>> GetTasksByWorker(string workerName)
//         {
//             try
//             {
//                 var tasks = await _workerTaskService.GetTasksByWorkerAsync(workerName);
//                 return Ok(tasks);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error retrieving tasks for worker {WorkerName}", workerName);
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//
//         [HttpGet("status/{status}")]
//         public async Task<ActionResult<IEnumerable<WorkerTaskDto>>> GetTasksByStatus(string status)
//         {
//             try
//             {
//                 var tasks = await _workerTaskService.GetTasksByStatusAsync(status);
//                 return Ok(tasks);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error retrieving tasks by status {Status}", status);
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//
//         [HttpGet("health-status/{healthStatus}")]
//         public async Task<ActionResult<IEnumerable<WorkerTaskDto>>> GetTasksByHealthStatus(string healthStatus)
//         {
//             try
//             {
//                 var tasks = await _workerTaskService.GetTasksByHealthStatusAsync(healthStatus);
//                 return Ok(tasks);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error retrieving tasks by health status {HealthStatus}", healthStatus);
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//
//         [HttpGet("health-summary")]
//         public async Task<ActionResult<Dictionary<string, int>>> GetWorkerHealthSummary()
//         {
//             try
//             {
//                 var summary = await _workerTaskService.GetWorkerHealthSummaryAsync();
//                 return Ok(summary);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error retrieving worker health summary");
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//
//         [HttpPost]
//         public async Task<ActionResult<WorkerTaskDto>> CreateTask([FromBody] CreateWorkerTaskDto createDto)
//         {
//             try
//             {
//                 if (!ModelState.IsValid)
//                     return BadRequest(ModelState);
//
//                 var task = await _workerTaskService.CreateTaskAsync(createDto);
//                 return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error creating task");
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//
//         [HttpPut("{id}")]
//         public async Task<ActionResult<WorkerTaskDto>> UpdateTask(int id, [FromBody] UpdateWorkerTaskDto updateDto)
//         {
//             try
//             {
//                 if (!ModelState.IsValid)
//                     return BadRequest(ModelState);
//
//                 var task = await _workerTaskService.UpdateTaskAsync(id, updateDto);
//                 return Ok(task);
//             }
//             catch (KeyNotFoundException)
//             {
//                 return NotFound($"Task with ID {id} not found");
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error updating task {TaskId}", id);
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//
//         [HttpDelete("{id}")]
//         public async Task<ActionResult> DeleteTask(int id)
//         {
//             try
//             {
//                 var deleted = await _workerTaskService.DeleteTaskAsync(id);
//                 if (!deleted)
//                     return NotFound($"Task with ID {id} not found");
//
//                 return NoContent();
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error deleting task {TaskId}", id);
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//     }
//     }
// }
