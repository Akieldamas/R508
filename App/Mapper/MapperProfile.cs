using AutoMapper;
using App.Models;
using App.DTO;
namespace App.Mapper;

public class MapperProfile: Profile
{
    public MapperProfile()
    {
        CreateMap<Produit, ProduitDto>()
    .ForMember(dest => dest.Type,
        opt => opt.MapFrom(src => src.TypeProduitNavigation != null ? src.TypeProduitNavigation.NomTypeProduit : null))
    .ForMember(dest => dest.Marque,
        opt => opt.MapFrom(src => src.MarqueNavigation != null ? src.MarqueNavigation.NomMarque : null))
    .ReverseMap();


    CreateMap<Produit, ProduitDetailDto>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProduit))
        .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.NomProduit))
        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
        .ForMember(dest => dest.Nomphoto, opt => opt.MapFrom(src => src.NomPhoto))
        .ForMember(dest => dest.Uriphoto, opt => opt.MapFrom(src => src.UriPhoto))
        .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockReel))
        .ForMember(dest => dest.EnReappro, opt => opt.MapFrom(src => (src.StockReel ?? 0) < src.StockMin))
        .ReverseMap();

    CreateMap<Marque, MarqueDto>()
        // IdMarque et NomMarque sont mappés automatiquement (même nom)
        .ForMember(dest => dest.NbProduits,
            opt => opt.MapFrom(src => src.Produits.Count)) // Produits.Count => NbProduits
        .ReverseMap()
        // Quand on reconvertit vers Marque, on n’a pas la liste des produits dans le DTO
        // donc on ignore cette propriété pour éviter les erreurs EF Core
        .ForMember(dest => dest.Produits, opt => opt.Ignore());


    CreateMap<TypeProduit, TypeProduitDto>().ReverseMap();
    }
}
