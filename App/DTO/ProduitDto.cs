namespace App.DTO;

public class ProduitDto
{
    public int IdProduit { get; set; }
    public string? NomProduit { get; set; }
    public string? Type { get; set; }
    public string? Marque { get; set; }

    protected bool Equals(ProduitDto other)
    {
        return NomProduit == other.NomProduit && Type == other.Type && Marque == other.Marque;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ProduitDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NomProduit, Type, Marque);
    }
}
