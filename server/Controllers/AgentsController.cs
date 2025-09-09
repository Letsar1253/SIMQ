using Microsoft.AspNetCore.Mvc;

namespace server.Controllers {

    [ApiController]
    [Route( "[controller]" )]
    public class AgentsController: ControllerBase {
        private readonly ILogger<AgentsController> _logger;

        public AgentsController( ILogger<AgentsController> logger ) {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAgentsList() {
            return Ok();
        }

        [HttpPost]
        public IActionResult RegisterAgent(
            [FromForm] List<IFormFile> files
        ) {
            return NoContent();
        }

        [HttpDelete( "{agent_name}" )]
        public IActionResult DeleteAgent(
            [FromRoute] string agent_name // Todo Поменять в API
        ) {
            return NoContent();
        }
    }
}
