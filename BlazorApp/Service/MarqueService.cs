using BlazorApp.Models;
using System.Net.Http.Json;
namespace BlazorApp.Service;

public class MarqueService : IService<Marque>
{
    private readonly HttpClient httpClient;

    public MarqueService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task AddAsync(Marque marque)
    {
        await httpClient.PostAsJsonAsync<Marque>("api/marque", marque);
    }

    public async Task DeleteAsync(int id)
    {
        await httpClient.DeleteAsync($"api/marque/{id}");
    }

    public async Task<List<Marque>?> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Marque>?>("api/marque");
    }

    public async Task<Marque?> GetByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<Marque?>($"api/marque/{id}");
    }
    public async Task UpdateAsync(Marque updatedMarque)
    {
        await httpClient.PutAsJsonAsync<Marque>($"api/marque", updatedMarque);
    }
}