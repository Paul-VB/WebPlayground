using WebPlayground.Core.Models.Ollama;

namespace WebPlayground.Core.Services
{
    public interface IOllamaService
    {
        IAsyncEnumerable<ChatResponse> Chat(string model, Message[] messages, bool stream = true);
    }

    public class OllamaService : IOllamaService
    {
        public OllamaService()
        {
        }
        public IAsyncEnumerable<ChatResponse> Chat(string model, Message[] messages, bool stream = true)
        {
            throw new NotImplementedException();
        }
    }
}
