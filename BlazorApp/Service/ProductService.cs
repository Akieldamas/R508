using BlazorApp.Models;
using System.Net.Http.Json;
namespace BlazorApp.Service;

public class ProductService : IService<Produit>
{
    private readonly HttpClient httpClient;

    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task AddAsync(Produit produit)
    {
        await httpClient.PostAsJsonAsync<Produit>("api/produits", produit);
    }

    public async Task DeleteAsync(int id)
    {
        await httpClient.DeleteAsync($"api/produits/{id}");
    }

    public async Task<List<Produit>?> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Produit>?>("api/produits");
    }

    public async Task<Produit?> GetByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<Produit?>($"api/produits/{id}");
    }
    public async Task UpdateAsync(Produit updatedEntity)
    {
        await httpClient.PutAsJsonAsync<Produit>($"api/produits", updatedEntity);
    }
}