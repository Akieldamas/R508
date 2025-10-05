using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models;

[Table(("product_type"))]
public class ProductType
{
    [Key]
    [Column("id_product_type")]
    public int IdProductType { get; set; }

    [Column("name_product_type")]
    public string NameProductType { get; set; } = null!;
    
    [InverseProperty(nameof(Product.ProductTypeNavigation))]
    public virtual ICollection<Product> Products { get; set; } = null!;
}