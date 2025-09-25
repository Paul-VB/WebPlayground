using Microsoft.AspNetCore.Mvc;
using WebPlayground.Data.ClientSqlJs;
using WebPlayground.Infrastructure.WebSocket.Services;

namespace WebPlayground.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketService _webSocketService;
        private readonly IClientSqlJsService clientSqlJsService; // we need this truss me

        public WebSocketController(IWebSocketService webSocketService, IClientSqlJsService clientSqlJsService)
        {
            _webSocketService = webSocketService;
            this.clientSqlJsService = clientSqlJsService;
        }

        [Route("initialize")]
        public async Task Initialize()
        {
            await _webSocketService.Initialize(HttpContext);
        }
    }
}