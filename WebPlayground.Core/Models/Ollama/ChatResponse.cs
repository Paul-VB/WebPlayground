namespace WebPlayground.Core.Models.Ollama
{
    public class ChatResponse
    {
        public string Model { get; set; }
        public DateTime CreatedAt { get; set; }
        public Message Message { get; set; }
        public bool Done { get; set; }
    }
}
