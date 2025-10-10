using AutoMapper;
using App.Models;
using App.DTO;
namespace App.Mapper;

public class ProductTypeMapper : Profile
{
    public ProductTypeMapper()
    {

        CreateMap<ProductType, ProductTypeDTO>()
         .ForMember(dest => dest.Products,
             opt => opt.MapFrom(src => src.Products.Count))
         .ReverseMap()
         .ForMember(dest => dest.Products, opt => opt.Ignore());
    }
}
