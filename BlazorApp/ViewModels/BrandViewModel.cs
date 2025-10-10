using BlazorApp.Models;
using BlazorApp.Service;
using BlazorBootstrap;
using System.Net.Http;
using System.Threading.Tasks;

public class BrandViewModel
{
    private readonly BrandService _service;
    private readonly ToastNotifications _toastNotifications;

    public IEnumerable<Brand> Brands { get; set; } = null;
    public BrandViewModel(BrandService service, ToastNotifications toastNotifications)
    {
        _service = service;
        _toastNotifications = toastNotifications;
    }

    public async Task<ToastMessage> LoadData()
    {
        List<Brand> brands = await _service.GetAllAsync();

        if (brands != null && brands.Any())
        {
            Brands = brands.ToList();
            return _toastNotifications.Create("Brands loaded successfully", ToastType.Success, "Success!");
        }

        return _toastNotifications.Create("Brands not found", ToastType.Warning, "Failed!");
    }

    public async Task<ToastMessage> CreateBrand(Brand marque)
    {
        try
        {
            List<Brand> marques = await _service.GetAllAsync();
            if (marques.Any(m => m.NameBrand.Equals(marque.NameBrand, StringComparison.OrdinalIgnoreCase)))
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

    public async Task<ToastMessage> DeleteBrand(Brand marque)
    {
        await _service.DeleteAsync(marque.IdBrand);
        await LoadData();
        return _toastNotifications.Create($"Deleted {marque.NameBrand}", ToastType.Danger, "Deleted");
    }

    public async Task<ToastMessage> UpdateBrand(Brand marque)
    {
        await _service.UpdateAsync(marque);
        await LoadData();
        return _toastNotifications.Create($"Updated {marque.NameBrand}", ToastType.Success, "Updated");
    }

}
