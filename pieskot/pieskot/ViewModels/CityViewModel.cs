using NaSpacerDo.LocalResource;
using System.ComponentModel.DataAnnotations;

namespace NaSpacerDo.ViewModels
{
    public class CityViewModel
    {
        public long Id { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resource))]
        public string Name { get; set; }
    }
}