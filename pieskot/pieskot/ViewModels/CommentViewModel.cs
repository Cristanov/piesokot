using System.ComponentModel.DataAnnotations;

namespace NaSpacerDo.ViewModels
{
    public class CommentViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Nick { get; set; }
    }
}