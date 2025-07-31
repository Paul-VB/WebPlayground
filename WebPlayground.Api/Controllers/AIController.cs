using Microsoft.AspNetCore.Mvc;
using WebPlayground.Core.Services;
using WebPlayground.Core.Models.Ollama;

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
        public IAsyncEnumerable<ChatResponse> Chat([FromBody] ChatRequest request)
        {
            return _ollamaService.Chat(request);
        }

        // Simple GET method that returns a ContentResult
        [HttpGet("test")]
        public IActionResult GetContent()
        {
            return new OkObjectResult("test from ai");
        }
    }
}
