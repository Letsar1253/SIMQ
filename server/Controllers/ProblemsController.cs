using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;

namespace server.Controllers {

    [ApiController]
    [Route( "[controller]" )]
    public class ProblemsController: ControllerBase {
        private readonly ILogger<ProblemsController> _logger;

        public ProblemsController( ILogger<ProblemsController> logger ) {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetProblemsList() {
            return Ok( ProblemsService.GetProblemsList() );
        }

        [HttpGet( "{problem_name}" )]
        public IActionResult GetProblemInfo(
            [FromRoute] string problem_name
        ) {
            problem_name = Uri.UnescapeDataString( problem_name );
            return Ok( ProblemsService.GetProblemInfo( problem_name ) );
        }

        [HttpDelete( "{problem_name}" )]
        public IActionResult DeleteProblem(
            [FromRoute] string problem_name
        ) {
            problem_name = Uri.UnescapeDataString( problem_name );
            ProblemsService.DeleteProblem( problem_name );
            return Ok();
        }

        [HttpPost]
        public IActionResult RegisterProblem(
            [FromBody] RegisterProblemRequest request
        ) {
            ProblemsService.RegisterProblem( request.data );
            return Ok();
        }

        [HttpGet( "{problem_name}/results" )]
        public IActionResult GetResults(
            [FromRoute] string problem_name
        ) {
            problem_name = Uri.UnescapeDataString( problem_name );
            return Ok( ProblemsService.GetResults( problem_name ) );
        }

        [HttpGet( "{problem_name}/results/{result_id}" )]
        public IActionResult GetResultInfo(
            [FromRoute] string problem_name,
            [FromRoute] Guid result_id
        ) {
            problem_name = Uri.UnescapeDataString( problem_name );
            return Ok( ProblemsService.GetResultInfo( problem_name, result_id ) );
        }

        [HttpDelete( "{problem_name}/results/{result_id}" )]
        public IActionResult DeleteResult(
            [FromRoute] string problem_name,
            [FromRoute] Guid result_id
        ) {
            problem_name = Uri.UnescapeDataString( problem_name );
            ProblemsService.DeleteResult(problem_name, result_id);
            return Ok();
        }
    }
}
