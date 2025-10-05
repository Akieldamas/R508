namespace App.DTO;

public class BrandDTO
{
    public int IdBrand { get; set; }
    public string? NameBrand { get; set; }
    public int NbProducts { get; set; }

    protected bool Equals(BrandDTO other)
    {
        return NameBrand == other.NameBrand && NameBrand == other.NameBrand;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((BrandDTO)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NameBrand, NbProducts);
    }
}
