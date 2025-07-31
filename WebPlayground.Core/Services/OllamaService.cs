using WebPlayground.Core.Models.Ollama;

namespace WebPlayground.Core.Services
{
    public interface IOllamaService
    {
        IAsyncEnumerable<ChatResponse> Chat(ChatRequest request);
    }

    public class OllamaService : IOllamaService
    {
        public OllamaService()
        {
        }
        public IAsyncEnumerable<ChatResponse> Chat(ChatRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
