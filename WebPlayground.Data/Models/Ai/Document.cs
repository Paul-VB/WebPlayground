using System.ComponentModel.DataAnnotations;

namespace WebPlayground.Data.Models.Ai
{
    public class Document : Entity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<DocumentChunk> Chunks { get; set; }

    }
}
