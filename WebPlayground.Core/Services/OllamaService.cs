using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using WebPlayground.Core.Exceptions;
using WebPlayground.Core.Wrappers;
using WebPlayground.Core.Models.Ollama;

namespace WebPlayground.Core.Services
{
    public interface IOllamaService
    {
        Task<Stream> Chat(ChatRequest request);
        Task<string> ListModels();
    }

    public class OllamaService : IOllamaService
    {
        private readonly ILogger<OllamaService> _logger;
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IConfigurationManager _configurationManager;

        public OllamaService(ILogger<OllamaService> logger, IHttpClientWrapper httpClientWrapper, IConfigurationManager configurationManager)
        {
            _logger = logger;
            _httpClientWrapper = httpClientWrapper;
            _configurationManager = configurationManager;
        }

        public async Task<Stream> Chat(ChatRequest request)
        {
            var url = $"{_configurationManager["OllamaApiUrl"]}/api/chat";
            var json = JsonSerializer.Serialize(request);

            _logger.LogInformation("Sending to Ollama: {Json}", json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            var response = await SendWithFriendlyErrorMessages(httpRequest, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();

        }

        public async Task<string> ListModels()
        {
            var url = $"{_configurationManager["OllamaApiUrl"]}/api/tags";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendWithFriendlyErrorMessages(httpRequest);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }

        private async Task<HttpResponseMessage> SendWithFriendlyErrorMessages(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClientWrapper.SendAsync(request, completionOption, cancellationToken);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                _logger.LogError(ex, "Ollama Host machine is running, but Ollama is not");
                throw new ServiceOfflineException("Ollama Host machine is running, but Ollama is not").MarkAsLogged();
            }
            catch (HttpRequestException ex) when (ex.StatusCode != null)
            {
                _logger.LogError(ex, "Ollama returned error status {StatusCode} for {Url}",
                    ex.StatusCode, request.RequestUri);
                throw new LoggableException($"Ollama returned an error: {ex.StatusCode}").MarkAsLogged();
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex,"Connection to Ollama Host timed out. The Host Machine may be off");
                throw new ServiceOfflineException("Connection to Ollama Host timed out. The Host Machine may be off").MarkAsLogged();
            }
        }
    }
}
