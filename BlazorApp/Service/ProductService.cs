using BlazorApp.Models;
using System.Net.Http.Json;
namespace BlazorApp.Service;

public class ProductService : IService<Product>
{
    private readonly HttpClient httpClient;

    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task AddAsync(Product produit)
    {
        await httpClient.PostAsJsonAsync<Product>("api/products", produit);
    }

    public async Task DeleteAsync(int id)
    {
        await httpClient.DeleteAsync($"api/products/{id}");
    }

    public async Task<List<Product>?> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Product>?>("api/products");
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<Product?>($"api/products/{id}");
    }
    public async Task UpdateAsync(Product updatedEntity)
    {
        await httpClient.PutAsJsonAsync<Product>($"api/products", updatedEntity);
    }
}