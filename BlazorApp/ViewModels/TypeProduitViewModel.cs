using BlazorApp.Models;
using BlazorApp.Service;
using BlazorBootstrap;
using System.Net.Http;
using System.Threading.Tasks;

public class TypeProduitViewModel
{
    private readonly TypeProduitService _service;
    private readonly ToastNotifications _toastNotifications;

    public IEnumerable<TypeProduit> TypeProduits { get; set; } = null;
    public TypeProduitViewModel(TypeProduitService service, ToastNotifications toastNotifications)
    {
        _service = service;
        _toastNotifications = toastNotifications;
    }

    public async Task<ToastMessage> LoadData()
    {
        List<TypeProduit> typeProduits = await _service.GetAllAsync();

        if (typeProduits != null && typeProduits.Any())
        {
            TypeProduits = typeProduits.ToList();
            return _toastNotifications.Create("Type de produits loaded successfully", ToastType.Success, "Success!");
        }

        return _toastNotifications.Create("Type de produits not found", ToastType.Warning, "Failed!");
    }

    public async Task<ToastMessage> CreateProduit(TypeProduit typeProduit)
    {
        try
        {
            await _service.AddAsync(typeProduit); // calls your API
            return _toastNotifications.Create("Type de produit added successfully!", ToastType.Success, "Success!");
        }
        catch (Exception ex)
        {
            return _toastNotifications.Create($"Failed to add type de produit: {ex.Message}", ToastType.Danger, "Error");
        }
    }

    public async Task<ToastMessage> DeleteProduit(TypeProduit typeProduit)
    {
        await _service.DeleteAsync(typeProduit.IdTypeProduit);
        await LoadData();
        return _toastNotifications.Create($"Deleted {typeProduit.NomTypeProduit}", ToastType.Danger, "Deleted");
    }

    public async Task<ToastMessage> UpdateProduit(TypeProduit typeProduit)
    {
        await _service.UpdateAsync(typeProduit);
        await LoadData();
        return _toastNotifications.Create($"Updated {typeProduit.NomTypeProduit}", ToastType.Success, "Updated");
    }

}
