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

        // Example endpoint for chat
        [HttpPost("chat")]
        public IAsyncEnumerable<ChatResponse> Chat([FromBody] Message[] messages, [FromQuery] string model = "llama2", [FromQuery] bool stream = true)
        {
            return _ollamaService.Chat(model, messages, stream);
        }
    }
}
