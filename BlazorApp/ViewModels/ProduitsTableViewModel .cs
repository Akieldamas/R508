using BlazorApp.Models;
using BlazorApp.Service;
using BlazorBootstrap;
using System.Net.Http;
using System.Threading.Tasks;

public class ProduitsTableViewModel
{
    private readonly ProductService _service;
    private readonly ToastNotifications _toastNotifications;

    public IEnumerable<Produit> Produits { get; set; } = null;
    public ProduitsTableViewModel(ProductService service, ToastNotifications toastNotifications)
    {
        _service = service;
        _toastNotifications = toastNotifications;
    }

    public async Task<ToastMessage> LoadData()
    {
        List<Produit> produits = await _service.GetAllAsync();

        if (produits != null && produits.Any())
        {
            Produits = produits.ToList();
            return _toastNotifications.Create("Products loaded successfully", ToastType.Success, "Success!");
        }

        return _toastNotifications.Create("Products not found", ToastType.Warning, "Failed!");
    }

    public async Task<ToastMessage> CreateProduit(Produit produit)
    {
        try
        {
            await _service.AddAsync(produit); // calls your API
            return _toastNotifications.Create("Product added successfully!", ToastType.Success, "Success!");
        }
        catch (Exception ex)
        {
            return _toastNotifications.Create($"Failed to add product: {ex.Message}", ToastType.Danger, "Error");
        }
    }

    public async Task<ToastMessage> DeleteProduit(Produit produit)
    {
        await _service.DeleteAsync(produit.IdProduit);
        await LoadData();
        return _toastNotifications.Create($"Deleted {produit.NomProduit}", ToastType.Danger, "Deleted");
    }

    public async Task<ToastMessage> UpdateProduit(Produit produit)
    {
        await _service.UpdateAsync(produit);
        await LoadData();
        return _toastNotifications.Create($"Updated {produit.NomProduit}", ToastType.Success, "Updated");
    }

}
