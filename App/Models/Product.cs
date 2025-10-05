using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace App.Models;

[Table(("product"))]
public class Product
{
    [Key]
    [Column("id_product")] 
    public int IdProduct { get; set; }

    [Column("name_product")] 
    public string NameProduct { get; set; } = null!;

    [Column("description")] 
    public string Description { get; set; } = null!;

    [Column("name_photo")] 
    public string NamePhoto { get; set; } = null!;

    [Column("uri_photo")] 
    public string UriPhoto { get; set; } = null!;

    [Column("id_product_type")] 
    public int? IdProductType { get; set; }

    [Column("id_brand")]
    public int? IdBrand { get; set; }

    [Column("stock_real")]
    public int? StockReal { get; set; }
    
    [Column("stock_min")]
    public int StockMin { get; set; }
    
    [Column("stock_max")]
    public int StockMax { get; set; }

    [ForeignKey(nameof(IdBrand))]
    [InverseProperty(nameof(Brand.Products))]
    [JsonIgnore]
    public virtual Brand? BrandNavigation { get; set; }
    
    [ForeignKey(nameof(IdProductType))]
    [InverseProperty(nameof(ProductType.Products))]
    [JsonIgnore]
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