using BlazorApp.Models;
using BlazorApp.Service;
using BlazorBootstrap;
using System.Net.Http;
using System.Threading.Tasks;

public class ProductTypeViewModel
{
    private readonly ProductTypeService _service;
    private readonly ToastNotifications _toastNotifications;

    public IEnumerable<ProductType> ProductTypes { get; set; } = null;
    public ProductTypeViewModel(ProductTypeService service, ToastNotifications toastNotifications)
    {
        _service = service;
        _toastNotifications = toastNotifications;
    }

    public async Task<ToastMessage> LoadData()
    {
        List<ProductType> productTypes = await _service.GetAllAsync();

        if (productTypes != null && productTypes.Any())
        {
            ProductTypes = productTypes.ToList();
            return _toastNotifications.Create("Product types loaded successfully", ToastType.Success, "Success!");
        }

        return _toastNotifications.Create("Product types not found", ToastType.Warning, "Failed!");
    }

    public async Task<ToastMessage> CreateProductType(ProductType productType)
    {
        try
        {
            await _service.AddAsync(productType); // calls your API
            return _toastNotifications.Create("Type de produit added successfully!", ToastType.Success, "Success!");
        }
        catch (Exception ex)
        {
            return _toastNotifications.Create($"Failed to add type de produit: {ex.Message}", ToastType.Danger, "Error");
        }
    }

    public async Task<ToastMessage> DeleteProductType(ProductType productType)
    {
        await _service.DeleteAsync(productType.IdProductType);
        await LoadData();
        return _toastNotifications.Create($"Deleted {productType.NameProductType}", ToastType.Danger, "Deleted");
    }

    public async Task<ToastMessage> UpdateProductType(ProductType productType)
    {
        await _service.UpdateAsync(productType);
        await LoadData();
        return _toastNotifications.Create($"Updated {productType.NameProductType}", ToastType.Success, "Updated");
    }

}
