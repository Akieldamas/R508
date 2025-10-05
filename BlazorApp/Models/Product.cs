using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Models;

public class Product
{

    public int IdProduct { get; set; }
    public string NameProduct { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string NamePhoto { get; set; } = null!;
    public string UriPhoto { get; set; } = null!;
    public int? IdProductType { get; set; }
    public int? IdBrand { get; set; }

    public int? StockReal { get; set; }

    public int StockMin { get; set; }

    public int StockMax { get; set; }

    public virtual Brand? BrandNavigation { get; set; }

    public virtual ProductType? ProductTypeNavigation { get; set; }

    

    private bool Equals(Product other)
    {
        return NameProduct == other.NameProduct;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Product)obj);
    }
}