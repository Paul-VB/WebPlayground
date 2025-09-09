using System.ComponentModel.DataAnnotations;

namespace WebPlayground.Data.Models
{
    public abstract class Entity
    {
        [Key]
        public long Id { get; set; }
    }
}
