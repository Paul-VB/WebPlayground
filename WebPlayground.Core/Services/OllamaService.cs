using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Text.Json;
using WebPlayground.Core.Helpers;
using WebPlayground.Core.Models.Ollama;

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
            var url = _configurationManager["OllamaApiUrl"];
            try
            {
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
                        content = $"AI Agent cannot be reached. It might be offline. attempted to contact it at: {url}"
                    }
                };
                var offlineJson = JsonSerializer.Serialize(offlineMessage);
                return new MemoryStream(Encoding.UTF8.GetBytes(offlineJson));
            }
        }
    }
}
