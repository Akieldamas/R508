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
        await httpClient.PostAsJsonAsync<Product>("api/produits", produit);
    }

    public async Task DeleteAsync(int id)
    {
        await httpClient.DeleteAsync($"api/produits/{id}");
    }

    public async Task<List<Product>?> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Product>?>("api/produits");
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<Product?>($"api/produits/{id}");
    }
    public async Task UpdateAsync(Product updatedEntity)
    {
        await httpClient.PutAsJsonAsync<Product>($"api/produits", updatedEntity);
    }
}