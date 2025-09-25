using System.Text.Json.Serialization;

namespace WebPlayground.Infrastructure.WebSocket.Models
{
    public class Message
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("payload")]
        public string? Payload { get; set; }

        [JsonPropertyName("eventType")]
        public string? EventType { get; set; } //if null, this is a response to a message

    }
}
