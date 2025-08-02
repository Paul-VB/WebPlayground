using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Text;
using System.Text.Json;
using WebPlayground.Core.Models.Ollama;
using WebPlayground.Core.Services;

namespace WebPlayground.FunctionApp.Functions
{
    public class AiFunctions
    {
        private readonly IOllamaService _ollamaService;

        public AiFunctions(IOllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        [Function("ChatFunction")]
        public async Task<HttpResponseData> Chat([HttpTrigger(AuthorizationLevel.Function, "post", "options", Route = "ai/chat")] HttpRequestData req)
        {
            var response = req.CreateResponse();

            response.Headers.Add("Content-Type", "application/json");
            var request = DeserializeHttpRequestData<ChatRequest>(req);
            try
            {
                using var stream = _ollamaService.Chat(request);
                await stream.CopyToAsync(response.Body);
            }
            catch (Exception ex)
            {
                var url = Environment.GetEnvironmentVariable("OllamaApiUrl") + "/api/chat";
                var offlineMessage = new
                {
                    message = new
                    {
                        role = "system",
                        content = $"AI Agent cannot be reached. It might be offline."
                    }
                };
                var offlineJson = JsonSerializer.Serialize(offlineMessage);
                await response.WriteStringAsync(offlineJson);
            }

            return response;
        }

        private T DeserializeHttpRequestData<T>(HttpRequestData req)
        {
            using var reader = new StreamReader(req.Body, Encoding.UTF8);
            string requestBodyAsJsonString = reader.ReadToEndAsync().Result;
            var result = JsonSerializer.Deserialize<T>(requestBodyAsJsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        } 
    }
}