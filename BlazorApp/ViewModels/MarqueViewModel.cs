using BlazorApp.Models;
using BlazorApp.Service;
using BlazorBootstrap;
using System.Net.Http;
using System.Threading.Tasks;

public class MarqueViewModel
{
    private readonly MarqueService _service;
    private readonly ToastNotifications _toastNotifications;

    public IEnumerable<Marque> Marques { get; set; } = null;
    public MarqueViewModel(MarqueService service, ToastNotifications toastNotifications)
    {
        _service = service;
        _toastNotifications = toastNotifications;
    }

    public async Task<ToastMessage> LoadData()
    {
        List<Marque> marques = await _service.GetAllAsync();

        if (marques != null && marques.Any())
        {
            Marques = marques.ToList();
            return _toastNotifications.Create("Marques loaded successfully", ToastType.Success, "Success!");
        }

        return _toastNotifications.Create("Marques not found", ToastType.Warning, "Failed!");
    }

    public async Task<ToastMessage> CreateProduit(Marque marque)
    {
        try
        {
            List<Marque> marques = await _service.GetAllAsync();
            if (marques.Any(m => m.NomMarque.Equals(marque.NomMarque, StringComparison.OrdinalIgnoreCase)))
            {
                return _toastNotifications.Create("Marque with the same name already exists!", ToastType.Danger, "Error");
            }
            await _service.AddAsync(marque);
            return _toastNotifications.Create("Marque added successfully!", ToastType.Success, "Success!");
        }
        catch (Exception ex)
        {
            return _toastNotifications.Create($"Failed to add product: {ex.Message}", ToastType.Danger, "Error");
        }
    }

    public async Task<ToastMessage> DeleteProduit(Marque marque)
    {
        await _service.DeleteAsync(marque.IdMarque);
        await LoadData();
        return _toastNotifications.Create($"Deleted {marque.NomMarque}", ToastType.Danger, "Deleted");
    }

    public async Task<ToastMessage> UpdateProduit(Marque marque)
    {
        await _service.UpdateAsync(marque);
        await LoadData();
        return _toastNotifications.Create($"Updated {marque.NomMarque}", ToastType.Success, "Updated");
    }

}
