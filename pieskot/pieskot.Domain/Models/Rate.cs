using NaSpacerDo.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace NaSpacerDo.Domain.Models
{
    public class Rate
    {
        public long Id { get; set; }

        [Required]
        public double Value { get; set; }

        [Required]
        public virtual Company Company { get; set; }
    }
}
