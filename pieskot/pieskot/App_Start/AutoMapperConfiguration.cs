using AutoMapper;
using NaSpacerDo.Domain.Models;
using NaSpacerDo.ViewModels;

namespace NaSpacerDo.App_Start
{
    public class AutoMapperConfiguration : Profile
    {
        protected override void Configure()
        {
            CreateMap<Company, CompanyViewModel>();
            CreateMap<CompanyViewModel, Company>();

            CreateMap<ImageViewModel, Image>()
                .ForMember(x => x.Content, cfg => cfg.MapFrom(y => ImageViewModel.FileToByteArray(y.Image)));
            CreateMap<Image, ImageViewModel>();

            CreateMap<AddressViewModel, Address>();
            CreateMap<Address, AddressViewModel>();

            CreateMap<CityViewModel, City>();
            CreateMap<City, CityViewModel>();

            CreateMap<CommentViewModel, Comment>();
            CreateMap<Comment, CommentViewModel>();

            CreateMap<RateViewModel, Rate>();
            CreateMap<Rate, RateViewModel>();
        }
    }
}