using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorApp.Models;

public class ProductType
{
    public int IdProductType { get; set; }
    public string NameProductType { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = null!;
}