using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace WebPlayground.Data.ClientSqlJs
{
    public interface IClientSqlJsService
    {
        Task InitializeConnection(HttpContext httpContext);
        Task<QueryResponse> QueryDb(Query query);
        Task HandleDbToServerResponse(QueryResponse response);
    }
    public class ClientSqlJsService : IClientSqlJsService
    {
        private readonly ILogger<ClientSqlJsService> _logger;

        private readonly TimeSpan _connectionTimeout = TimeSpan.FromMinutes(2);
        private readonly TimeSpan _heartbeatInterval = TimeSpan.FromSeconds(10);

        private DateTime _lastActivityUtc { get; set; } = DateTime.UtcNow;
        private Dictionary<Guid, TaskCompletionSource<QueryResponse>> _pendingQueries = new();
        private HttpContext? _activeConnectionContext;
        public ClientSqlJsService(ILogger<ClientSqlJsService> logger)
        {
            this._logger = logger;
        }

        public async Task InitializeConnection(HttpContext httpContext)
        {
            _activeConnectionContext = httpContext;
            var response = httpContext.Response;
            response.Headers.Append("Content-Type", "text/event-stream");
            response.Headers.Append("Cache-Control", "no-cache");
            response.Headers.Append("Connection", "keep-alive");

            while (!httpContext.RequestAborted.IsCancellationRequested)
            {
                if (DateTime.UtcNow - _lastActivityUtc > _connectionTimeout) break;

                await Task.Delay(_heartbeatInterval);
                await SendSseComment("heartbeat");
            }
            _activeConnectionContext = null;
            _pendingQueries.Clear();
            return;
        }

        public async Task<QueryResponse> QueryDb(Query query)
        {
            var promise = new TaskCompletionSource<QueryResponse>();
            _pendingQueries[query.QueryId] = promise;

            var queryJson = JsonSerializer.Serialize(query);
            await SendSseData(queryJson);

            var response = await promise.Task;
            _pendingQueries.Remove(query.QueryId);
            return response;
        }

        public Task HandleDbToServerResponse(QueryResponse response)
        {
            UpdateLastActivityUtc();

            try
            {
                var pendingPromise = _pendingQueries[response.QueryId];
                pendingPromise.SetResult(response);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "No pending query found for QueryId: {QueryId}", response.QueryId);
            }

            return Task.CompletedTask;
        }
        private void UpdateLastActivityUtc() => _lastActivityUtc = DateTime.UtcNow; 
        private async Task SendSseData(string messageJson)
        {
            if (_activeConnectionContext == null) throw new InvalidOperationException("No active connection");
            var response = _activeConnectionContext.Response;
            await response.WriteAsync($"data: {messageJson}\n\n");
            await response.Body.FlushAsync();
        }

        private async Task SendSseComment(string comment)
        {
            if (_activeConnectionContext == null) throw new InvalidOperationException("No active connection");
            var response = _activeConnectionContext.Response;
            await response.WriteAsync($": {comment}\n\n");
            await response.Body.FlushAsync();
        }
    }
    //todo move these to their own files
    public class Query
    {
        public Guid QueryId { get; set; } = Guid.NewGuid();
    }
    public class QueryResponse
    {
        public Guid QueryId { get; set; }
    }
}
