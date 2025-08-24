using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebPlayground.Core.Exceptions;
using WebPlayground.Core.Models.Ollama;
using WebPlayground.Core.Services;

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
            try
            {
                Response.ContentType = "application/json";
                using var stream = await _ollamaService.Chat(request);
                await stream.CopyToAsync(Response.Body);
            } catch (ServiceOfflineException ex)
            {
                Response.StatusCode = 503; // Service Unavailable
                await Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes($"{{\"error\":\"{ex.Message}\"}}"));
            }
        }

        [HttpGet("tags")]
        public async Task<IActionResult> ListModels()
        {
            try
            {
                var models = await _ollamaService.ListModels();
                return new OkObjectResult(models);
            }
            catch (ServiceOfflineException ex)
            {
                return StatusCode(503, new { error = ex.Message });
            }
        }

            // Simple GET method that returns a ContentResult
            [HttpGet("test")]
            public IActionResult GetContent()
            {
                return new OkObjectResult("test from ai");
            }
        }
    }
