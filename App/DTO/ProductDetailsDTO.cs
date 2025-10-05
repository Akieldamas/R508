namespace App.DTO;

public class ProductDetailsDTO
{
    public int Id { get; set; }
    public string? NameProduct { get; set; }
    public string? Type { get; set; }
    public string? Brand { get; set; }
    public string? Description { get; set; }
    public string? NamePhoto { get; set; }
    public string? UriPhoto { get; set; }
    public int? Stock { get; set; }
    public bool EnReappro { get; set; }

    protected bool Equals(ProductDetailsDTO other)
    {
        return NameProduct == other.NameProduct && Type == other.Type && Brand == other.Brand && Description == other.Description && NamePhoto == other.NamePhoto && UriPhoto == other.UriPhoto && Stock == other.Stock && EnReappro == other.EnReappro;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ProductDetailsDTO)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NameProduct, Type, Brand, Description, NamePhoto, UriPhoto, Stock, EnReappro);
    }
}
