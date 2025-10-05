using App.Models;

namespace App.DTO;

public class ProductTypeDTO
{
    public int IdProductType { get; set; }
    public string? NameProductType { get; set; }
    public int Products { get; set; }

    protected bool Equals(ProductTypeDTO other)
    {
        return NameProductType == other.NameProductType && Products == other.Products;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ProductTypeDTO)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NameProductType, Products);
    }
}