using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Models;

public class Brand
{

    public int IdBrand { get; set; }
    public string NameBrand { get; set; } = null!;
    public virtual ICollection<Product> Products { get; set; } = null!;
}