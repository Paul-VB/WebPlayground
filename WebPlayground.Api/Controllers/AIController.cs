using Microsoft.AspNetCore.Mvc;
using WebPlayground.Core.Models.Ollama;
using WebPlayground.Core.Services;
using System.Threading.Tasks;

namespace WebPlayground.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IOllamaService _ollamaService;

        public AIController(IOllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        [HttpPost("chat")]
        public async Task Chat([FromBody] ChatRequest request)
        {
            Response.ContentType = "application/json";
            using var stream = _ollamaService.Chat(request);
            await stream.CopyToAsync(Response.Body);
        }

        // Simple GET method that returns a ContentResult
        [HttpGet("test")]
        public IActionResult GetContent()
        {
            return new OkObjectResult("test from ai");
        }
    }
}
