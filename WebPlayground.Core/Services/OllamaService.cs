using WebPlayground.Core.Models.Ollama;
using WebPlayground.Core.Helpers;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace WebPlayground.Core.Services
{
    public interface IOllamaService
    {
        Stream Chat(ChatRequest request);
    }

    public class OllamaService : IOllamaService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IConfigurationManager _configurationManager;

        public OllamaService(IHttpClientWrapper httpClientWrapper, IConfigurationManager configurationManager)
        {
            _httpClientWrapper = httpClientWrapper;
            _configurationManager = configurationManager;
        }

        public Stream Chat(ChatRequest request)
        {
            try
            {
                var url = _configurationManager["OllamaApiUrl"];
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                var response = _httpClientWrapper.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
            }
            catch
            {
                var offlineMessage = new
                {
                    message = new
                    {
                        role = "system",
                        content = "AI Agent cannot be reached. It might be offline"
                    }
                };
                var offlineJson = JsonSerializer.Serialize(offlineMessage);
                return new MemoryStream(Encoding.UTF8.GetBytes(offlineJson));
            }
        }
    }
}
