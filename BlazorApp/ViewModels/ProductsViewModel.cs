using BlazorApp.Models;
using BlazorApp.Service;
using BlazorBootstrap;
using System.Net.Http;
using System.Threading.Tasks;

public class ProductsViewModel
{
    private readonly ProductService _service;
    private readonly ToastNotifications _toastNotifications;

    public IEnumerable<Product> Products { get; set; } = null;
    public ProductsViewModel(ProductService service, ToastNotifications toastNotifications)
    {
        _service = service;
        _toastNotifications = toastNotifications;
    }

    public async Task<ToastMessage> LoadData()
    {
        List<Product> products = await _service.GetAllAsync();

        if (products != null && products.Any())
        {
            Products = products.ToList();
            return _toastNotifications.Create("Products loaded successfully", ToastType.Success, "Success!");
        }

        return _toastNotifications.Create("Products not found", ToastType.Warning, "Failed!");
    }

    public async Task<ToastMessage> CreateProduit(Product product)
    {
        try
        {
            await _service.AddAsync(product); // calls your API
            return _toastNotifications.Create("Product added successfully!", ToastType.Success, "Success!");
        }
        catch (Exception ex)
        {
            return _toastNotifications.Create($"Failed to add product: {ex.Message}", ToastType.Danger, "Error");
        }
    }

    public async Task<ToastMessage> DeleteProduit(Product product)
    {
        await _service.DeleteAsync(product.IdProduct);
        await LoadData();
        return _toastNotifications.Create($"Deleted {product.NameProduct}", ToastType.Danger, "Deleted");
    }

    public async Task<ToastMessage> UpdateProduit(Product product)
    {
        await _service.UpdateAsync(product);
        await LoadData();
        return _toastNotifications.Create($"Updated {product.NameProduct}", ToastType.Success, "Updated");
    }

}
