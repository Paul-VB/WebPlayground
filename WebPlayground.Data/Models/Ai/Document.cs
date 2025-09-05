using System.ComponentModel.DataAnnotations;

namespace WebPlayground.Data.Models.Ai
{
    public class Document
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<DocumentChunk> Chunks { get; set; }

    }
}
