using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebPlayground.Core.Exceptions;
using WebPlayground.Infrastructure.WebSocket.Models;

namespace WebPlayground.Infrastructure.WebSocket.Services
{
    public interface IWebSocketService
    {
        Task Initialize(HttpContext httpContext);
        event Action OnInitialize;
        Task<string> SendAsync(string eventType, string payload);
        void OnReceive(string eventType, Func<string, string> handler);
    }
    public class WebSocketService : IWebSocketService
    {
        private readonly ILogger<WebSocketService> _logger;

        private readonly TimeSpan _inactiveConnectionTimeout = TimeSpan.FromMinutes(2);
        private readonly TimeSpan _receiveTimeout = TimeSpan.FromSeconds(30);

        private System.Net.WebSockets.WebSocket _webSocket = null!;
        private DateTime _lastActivityUtc = DateTime.UtcNow;
        private ConcurrentDictionary<string, TaskCompletionSource<Message>> _pendingMessages = new();
        private List<(string eventType, Func<string, string> handler)> _onReceiveHandlers = new();

        public WebSocketService(ILogger<WebSocketService> logger)
        {
            _logger = logger;
        }

        public async Task Initialize(HttpContext httpContext)
        {
            if (!httpContext.WebSockets.IsWebSocketRequest)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            _webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
            OnInitialize?.Invoke();
            await MaintainWebsocketUntilClose();
        }

        public event Action? OnInitialize;

        private async Task MaintainWebsocketUntilClose()
        {
            while (!WebSocketShouldClose())
            {
                var message = await ReceiveAsync(_receiveTimeout);
                if (message != null)
                {
                    await HandleMessage(message);
                }
            }
        }

        private bool WebSocketShouldClose()
        {
            if (_webSocket.State != WebSocketState.Open) return true;
            if (DateTime.UtcNow - _lastActivityUtc > _inactiveConnectionTimeout) return true;
            return false;

        }

        private async Task<Message?> ReceiveAsync(TimeSpan timeout)
        {
            var cancellationToken = new CancellationTokenSource(timeout).Token;

            var buffer = new byte[1024 * 4];
            string messageJson = null!;
            Message? message = null;
            try
            {
                var result = await _webSocket.ReceiveAsync(buffer, cancellationToken);
                //todo: maybe handle close message? we will see if this is needed later. if we test and it works, please remove this comment.
                _lastActivityUtc = DateTime.UtcNow;
                messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                message = JsonSerializer.Deserialize<Message>(messageJson);
            }
            catch (OperationCanceledException)
            {
                // Expected when no message is received within the timeout
            }
            catch (JsonException ex)
            {
                var loggableException = new LoggableException($"Failed to deserialize WebSocket message: {messageJson}", ex);
                _logger.LogError(loggableException, "Invalid JSON received from WebSocket client: {messageJson}", messageJson);
                loggableException.MarkAsLogged();
            }
            return message;

        }

        private async Task HandleMessage(Message message)
        {
            var messageIsRequestToServer = !string.IsNullOrEmpty(message.EventType);
            if (messageIsRequestToServer)
            {
                await HandleRequestToServer(message);
            }
            else
            {
                HandleResponseFromClient(message);
            }
        }

        private async Task HandleRequestToServer(Message message)
        {
            foreach (var onReceiveHandler in _onReceiveHandlers)
            {
                if (onReceiveHandler.eventType == message.EventType)
                {
                    var responsePayload = onReceiveHandler.handler(message.Payload ?? "");
                    var responseMessage = new Message()
                    {
                        Id = message.Id,
                        Payload = responsePayload
                    };
                    var responseMessageJson = JsonSerializer.Serialize(responseMessage);
                    await SendJson(responseMessageJson);
                }
            }
        }

        private void HandleResponseFromClient(Message message)
        {
            if (!_pendingMessages.ContainsKey(message.Id))
                throw new KeyNotFoundException($"there is no pending message with the id: {message.Id}");
            var pendingMessage = _pendingMessages[message.Id];
            pendingMessage.SetResult(message);
        }

        public async Task<string> SendAsync(string eventType, string payload)
        {
            if (_webSocket.State != WebSocketState.Open)
                throw new Exception("WebSocket is not open");

            var message = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                EventType = eventType,
                Payload = payload
            };
            var messageJson = JsonSerializer.Serialize(message);

            var pendingMessageResponse = new TaskCompletionSource<Message>();
            _pendingMessages[message.Id] = pendingMessageResponse;

            _lastActivityUtc = DateTime.UtcNow;
            await SendJson(messageJson);
            try
            {
                var response = await pendingMessageResponse.Task.WaitAsync(_receiveTimeout);
                return response.Payload!;
            }
            catch (TimeoutException ex)
            {
                //todo: maybe we should do something here other than throw. like somehow tell the caller it timed out. maybe we need a messageResult class?
                return null;
            }
            finally
            {
                _pendingMessages.Remove(message.Id, out _);
            }
        }

        private async Task SendJson(string json)
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            await _webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void OnReceive(string eventType, Func<string, string> handler)
        {
            _onReceiveHandlers.Add((eventType, handler));
        }

    }
}
