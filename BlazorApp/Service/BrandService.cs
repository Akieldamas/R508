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
        await httpClient.PostAsJsonAsync<Brand>("api/brands", marque);
    }

    public async Task DeleteAsync(int id)
    {
        await httpClient.DeleteAsync($"api/brands/{id}");
    }

    public async Task<List<Brand>?> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Brand>?>("api/brands");
    }

    public async Task<Brand?> GetByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<Brand?>($"api/brands/{id}");
    }
    public async Task UpdateAsync(Brand updatedMarque)
    {
        await httpClient.PutAsJsonAsync<Brand>($"api/brands", updatedMarque);
    }
}