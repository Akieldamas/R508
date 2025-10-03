using App.Models;

namespace App.DTO;

public class TypeProduitDto
{
    public int IdTypeProduit { get; set; }
    public string? NomTypeProduit { get; set; }
    public int Produits { get; set; }

    protected bool Equals(TypeProduitDto other)
    {
        return NomTypeProduit == other.NomTypeProduit && Produits == other.Produits;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TypeProduitDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NomTypeProduit, Produits);
    }
}