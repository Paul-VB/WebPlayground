using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPlayground.Data.Models.Ai
{
    public class DocumentChunk : Entity
    {
        public long DocumentFk { get; set; }
        [ForeignKey(nameof(DocumentFk))]
        public Document Document { get; set; }
        public int Index { get; set; }
        public string Content { get; set; }
        public float[] Embedding { get; set; }

    }
}
