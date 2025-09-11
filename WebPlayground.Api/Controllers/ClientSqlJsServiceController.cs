using Microsoft.AspNetCore.Mvc;
using WebPlayground.Data.ClientSqlJs;

namespace WebPlayground.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientSqlJsServiceController : ControllerBase
    {

        private readonly IClientSqlJsService _clientSqlJsService;
        public ClientSqlJsServiceController(IClientSqlJsService clientSqlJsService)
        {
            _clientSqlJsService = clientSqlJsService;
        }

        [HttpGet("serverToDb")]
        public async Task GetServerToDbStream()
        {
            await _clientSqlJsService.InitializeConnection(HttpContext);
            return;
        }

        [HttpPost("dbToServer")]
        public async Task<IActionResult> DbToServerResponse([FromBody] QueryResponse response)
        {
            await _clientSqlJsService.HandleDbToServerResponse(response);
            return Ok();
        }
    }
}
