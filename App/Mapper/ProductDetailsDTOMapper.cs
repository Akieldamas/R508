using AutoMapper;
using App.Models;
using App.DTO;
namespace App.Mapper;

public class ProductDetailsDTOMapper : Profile
{
    public ProductDetailsDTOMapper()
    {
        CreateMap<Product, ProductDetailsDTO>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ProductTypeNavigation.NameProductType ?? string.Empty))
        .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.BrandNavigation.NameBrand ?? String.Empty))
        .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockReal))
        .ForMember(dest => dest.EnReappro, opt => opt.MapFrom(src => (src.StockReal ?? 0) < src.StockMin))
        .ReverseMap()
        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => string.Empty))
        .ForMember(dest => dest.NamePhoto, opt => opt.MapFrom(src => string.Empty))
        .ForMember(dest => dest.UriPhoto, opt => opt.MapFrom(src => string.Empty))
        .ForMember(d => d.ProductTypeNavigation, o => o.Ignore())
        .ForMember(d => d.BrandNavigation, o => o.Ignore());
    }
}
