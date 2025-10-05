using AutoMapper;
using App.Models;
using App.DTO;
namespace App.Mapper;

public class MapperProfile: Profile
{
    public MapperProfile()
    {
        CreateMap<Product, ProductDTO>()
    .ForMember(dest => dest.Type,
        opt => opt.MapFrom(src => src.ProductTypeNavigation != null ? src.ProductTypeNavigation.NameProductType : null))
    .ForMember(dest => dest.Brand,
        opt => opt.MapFrom(src => src.BrandNavigation != null ? src.BrandNavigation.NameBrand : null))
    .ReverseMap();


    CreateMap<Product, ProductDetailsDTO>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProduct))
        .ForMember(dest => dest.NameProduct, opt => opt.MapFrom(src => src.NameProduct))
        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
        .ForMember(dest => dest.NamePhoto, opt => opt.MapFrom(src => src.NamePhoto))
        .ForMember(dest => dest.UriPhoto, opt => opt.MapFrom(src => src.UriPhoto))
        .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockReal))
        .ForMember(dest => dest.EnReappro, opt => opt.MapFrom(src => (src.StockReal ?? 0) < src.StockMin))
        .ReverseMap();

    CreateMap<Brand, BrandDTO>()
        // IdBrand et NomMarque sont mappés automatiquement (même nom)
        .ForMember(dest => dest.NbProducts,
            opt => opt.MapFrom(src => src.Products.Count)) // Produits.Count => NbProduits
        .ReverseMap()
        // Quand on reconvertit vers Marque, on n’a pas la liste des produits dans le DTO
        // donc on ignore cette propriété pour éviter les erreurs EF Core
        .ForMember(dest => dest.Products, opt => opt.Ignore());


    CreateMap<ProductType, ProductTypeDTO>().ReverseMap();
    }
}
