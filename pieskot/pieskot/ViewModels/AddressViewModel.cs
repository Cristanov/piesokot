using NaSpacerDo.LocalResource;
using System.ComponentModel.DataAnnotations;

namespace NaSpacerDo.ViewModels
{
    public class AddressViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Street", ResourceType = typeof(Resource))]
        public string Street { get; set; }

        [Display(Name = "AddressNumber", ResourceType = typeof(Resource))]
        public string Number { get; set; }

        [Display(Name = "ZipCode", ResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        public virtual CityViewModel City { get; set; }
    }
}