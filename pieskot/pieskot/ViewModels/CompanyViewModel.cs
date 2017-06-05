using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using NaSpacerDo.LocalResource;
using System.Linq;
using System.Collections.Generic;

namespace NaSpacerDo.ViewModels
{
    public class CompanyViewModel
    {
        public CompanyViewModel()
        {
            Images = new List<ImageViewModel>();
            Ratings = new List<RateViewModel>();
        }

        public long Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [MinLength(3, ErrorMessageResourceName = "MinLengthErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [Remote("Exist", "Company", AdditionalFields = nameof(Id), ErrorMessageResourceName = "CompanyAlreadyExistsErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "CompanyName", ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Phone(ErrorMessageResourceName = "InvalidPhoneErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Phone", ResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [Display(Name = "Website", ResourceType = typeof(Resource))]
        public string Www { get; set; }

        [MaxLength(150, ErrorMessageResourceName = "MaxLengthErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "ShortDescription", ResourceType = typeof(Resource))]
        public string ShortDescription { get; set; }

        [AllowHtml]
        [Display(Name = "LongDescription", ResourceType = typeof(Resource))]
        public string LongDescription { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual AddressViewModel Address { get; set; }

        [Display(Name = "Logo", ResourceType = typeof(Resource))]
        public ImageViewModel Logo
        {
            get { return Images.FirstOrDefault(x => x.IsLogo); }

            set
            {
                var logo = value;
                logo.IsLogo = true;
                Images.Add(logo);
            }
        }

        public ICollection<ImageViewModel> Images { get; set; }

        public ICollection<RateViewModel> Ratings { get; set; }

        public string OwnerId { get; set; }

        public double AverageRate
        {
            get
            {
                double averageRate = Ratings.Sum(x => x.Value) / Ratings.Count;
                return Math.Round(averageRate, 1);
            }
        }
    }
}