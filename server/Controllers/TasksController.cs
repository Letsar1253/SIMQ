using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;

namespace server.Controllers {

    [ApiController]
    [Route( "[controller]" )]
    public class TasksController: ControllerBase {
        private readonly ILogger<TasksController> _logger;

        public TasksController( ILogger<TasksController> logger ) {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetTaskList() {
            return Ok( TasksService.GetTaskList() );
        }

        [HttpGet( "{task_id}" )]
        public IActionResult GetTaskInfo(
            [FromRoute] uint task_id
        ) {
            _logger.LogError( $"GetTaskInfo, task_id: { ModelState.IsValid }" );

            return Ok( TasksService.GetTaskInfo( task_id ) );
        }

        [HttpPost]
        public IActionResult CreateTask(
            [FromBody] CreateTaskRequest request
        ) {
            return Ok( TasksService.AddTask( request.data ) );
        }

        [HttpPost( "{task_id}" )]
        public IActionResult StopTask(
            [FromRoute] uint task_id
        ) {
            TasksService.StopTask( task_id );
            return NoContent();
        }
    }
}
