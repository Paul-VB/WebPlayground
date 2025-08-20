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
        Task<Stream> Chat(ChatRequest request);
        Task<string> ListModels();
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

        public async Task<Stream> Chat(ChatRequest request)
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
                response = await _httpClientWrapper.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework you prefer)
                Console.WriteLine($"Error occurred while calling Ollama API: {ex.Message}");
                throw;
            }
        }

        public async Task<string> ListModels()
        {
            var url = $"{_configurationManager["OllamaApiUrl"]}/api/tags";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage response;
            try
            {
                response = await _httpClientWrapper.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework you prefer)
                Console.WriteLine($"Error occurred while listing models: {ex.Message}");
                throw;
            }
        }
    }
}
