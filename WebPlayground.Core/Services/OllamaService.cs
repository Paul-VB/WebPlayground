using WebPlayground.Core.Models.Ollama;
using WebPlayground.Core.Helpers;
using System.Text.Json;
using System.Text;

namespace WebPlayground.Core.Services
{
    public interface IOllamaService
    {
        Stream Chat(ChatRequest request);
    }

    public class OllamaService : IOllamaService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private const string OllamaApiUrl = "http://localhost:11434/api/chat";

        public OllamaService(IHttpClientWrapper httpClientWrapper)
        {
            _httpClientWrapper = httpClientWrapper;
        }

        public Stream Chat(ChatRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, OllamaApiUrl)
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
