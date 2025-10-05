using BlazorApp.Models;
using System.Net.Http.Json;
namespace BlazorApp.Service;

public class BrandService : IService<Brand>
{
    private readonly HttpClient httpClient;

    public BrandService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task AddAsync(Brand marque)
    {
        await httpClient.PostAsJsonAsync<Brand>("api/marque", marque);
    }

    public async Task DeleteAsync(int id)
    {
        await httpClient.DeleteAsync($"api/marque/{id}");
    }

    public async Task<List<Brand>?> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Brand>?>("api/marque");
    }

    public async Task<Brand?> GetByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<Brand?>($"api/marque/{id}");
    }
    public async Task UpdateAsync(Brand updatedMarque)
    {
        await httpClient.PutAsJsonAsync<Brand>($"api/marque", updatedMarque);
    }
}