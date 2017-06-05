using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaSpacerDo.Domain.Models
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Nick { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        public virtual Company Company { get; set; }
    }
}
