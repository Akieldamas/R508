using BlazorApp.Models;
using System.Net.Http.Json;
namespace BlazorApp.Service;

public class TypeProduitService : IService<TypeProduit>
{
    private readonly HttpClient httpClient;

    public TypeProduitService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task AddAsync(TypeProduit typeProduit)
    {
        await httpClient.PostAsJsonAsync<TypeProduit>("api/typeproduit", typeProduit);
    }

    public async Task DeleteAsync(int id)
    {
        await httpClient.DeleteAsync($"api/typeproduit/{id}");
    }

    public async Task<List<TypeProduit>?> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<TypeProduit>?>("api/typeproduit");
    }

    public async Task<TypeProduit?> GetByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<TypeProduit?>($"api/typeproduit/{id}");
    }
    public async Task UpdateAsync(TypeProduit updatedTypeProduct)
    {
        await httpClient.PutAsJsonAsync<TypeProduit>($"api/typeproduit", updatedTypeProduct);
    }
}