using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models;

[Table(("marque"))]
public class Marque
{
    [Key]
    [Column("id_marque")]
    public int IdMarque { get; set; }

    [Column("nom_marque")] 
    public string NomMarque { get; set; } = null!;

    [InverseProperty(nameof(Produit.MarqueNavigation))]
    public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
}