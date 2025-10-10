using AutoMapper;
using App.Models;
using App.DTO;
namespace App.Mapper;

public class BrandMapper : Profile
{
    public BrandMapper()
    {
        CreateMap<Brand, BrandDTO>()
            .ForMember(dest => dest.NbProducts,
                opt => opt.MapFrom(src => src.Products.Count))
            .ReverseMap()
            .ForMember(dest => dest.Products, opt => opt.Ignore());
    }
}
