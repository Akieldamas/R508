using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Models;

public class ProductType
{
    public int IdProductType { get; set; }
    public string NameProductType { get; set; } = null!;
    public virtual ICollection<Product> Products { get; set; } = null!;
}