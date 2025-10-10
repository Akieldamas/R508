using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models;

[Table(("brand"))]
public class Brand
{
    [Key]
    [Column("id_brand")]
    public int IdBrand { get; set; }

    [Column("name_brand")] 
    public string NameBrand { get; set; } = null!;

    [InverseProperty(nameof(Product.BrandNavigation))]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}