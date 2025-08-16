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
            var url = $"{_configurationManager["OllamaApiUrl"]}/api/chat";
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            HttpResponseMessage response;
            try
            {
                response = _httpClientWrapper.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework you prefer)
                Console.WriteLine($"Error occurred while calling Ollama API: {ex.Message}");
                throw;
            }
        }
    }
}
