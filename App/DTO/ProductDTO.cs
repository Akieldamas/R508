namespace App.DTO;

public class ProductDTO
{
    public int IdProduct { get; set; }
    public string? NameProduct { get; set; }
    public string? Type { get; set; }
    public string? Brand { get; set; }

    protected bool Equals(ProductDTO other)
    {
        return NameProduct == other.NameProduct && Type == other.Type && Brand == other.Brand;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ProductDTO)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NameProduct, Type, Brand);
    }
}
