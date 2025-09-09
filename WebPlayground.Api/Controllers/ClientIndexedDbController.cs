using Microsoft.AspNetCore.Mvc;
using WebPlayground.Data.ClientIndexedDb;

namespace WebPlayground.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientIndexedDbController : ControllerBase
    {

        private readonly IClientIndexedDbService _clientIndexedDbService;
        public ClientIndexedDbController(IClientIndexedDbService clientIndexedDbService)
        {
            _clientIndexedDbService = clientIndexedDbService;
        }

        [HttpGet("serverToDb")]
        public async Task GetServerToDbStream()
        {
            await _clientIndexedDbService.InitializeConnection(HttpContext);
            return;
        }

        [HttpPost("dbToServer")]
        public async Task<IActionResult> DbToServerResponse([FromBody] QueryResponse response)
        {
            await _clientIndexedDbService.HandleDbToServerResponse(response);
            return Ok();
        }
    }
}
