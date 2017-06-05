using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaSpacerDo.Domain.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string FileName { get; set; }

        public string Path { get; set; }

        [Required]
        public bool IsLogo { get; set; }

        [NotMapped]
        public byte[] Content { get; set; }

        [Required]
        public virtual Company Company { get; set; }
    }
}