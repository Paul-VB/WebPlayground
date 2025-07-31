namespace WebPlayground.Core.Models.Ollama
{
    public class ChatRequest
    {
        public string Model { get; set; }
        public Message[] Messages { get; set; }
        public bool Stream { get; set; } = true;
        // Add other properties like Options, Context, KeepAlive as needed
    }
}
