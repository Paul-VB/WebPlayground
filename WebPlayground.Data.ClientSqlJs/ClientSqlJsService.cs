using Microsoft.AspNetCore.Http;
using System.Text.Json;
using WebPlayground.Infrastructure.WebSocket.Services;

namespace WebPlayground.Data.ClientSqlJs
{
    public interface IClientSqlJsService
    {
        Task LoadDb(string dbName);
        Task SaveDb();
        Task CreateDbIfNotExists(string dbName);
        Task<object> Query(string sql, object[] parameters = null);
        Task<int> NonQuery(string sql, object[] parameters = null);
    }
    public class ClientSqlJsService : IClientSqlJsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebSocketService _webSocketService;
        public ClientSqlJsService(IHttpContextAccessor httpContextAccessor, IWebSocketService webSocketService)
        {
            _httpContextAccessor = httpContextAccessor;
            _webSocketService = webSocketService;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            async void init()
            {
                await CreateDbIfNotExists("webplayground_dev"); //todo: make configurable
                await LoadDb("webplayground_dev");
            }

            _webSocketService.OnInitialize += init;

        }

        public async Task LoadDb(string dbName)
        {
            var Payload = JsonSerializer.Serialize(new { dbName });
            var response = await _webSocketService.SendAsync("loadDb", Payload);
        }

        public async Task SaveDb()
        {
            var Payload = "{}";
            var response = await _webSocketService.SendAsync("loadDb", Payload);
        }

        public async Task CreateDbIfNotExists(string dbName)
        {
            var Payload = JsonSerializer.Serialize(new { dbName });
            var response = await _webSocketService.SendAsync("createDbIfNotExists", Payload);
        }

        public async Task<object> Query(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }

        public async Task<int> NonQuery(string sql, object[] parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
