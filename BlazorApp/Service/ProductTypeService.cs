using BlazorApp.Models;
using System.Net.Http.Json;
namespace BlazorApp.Service;

public class ProductTypeService : IService<ProductType>
{
    private readonly HttpClient httpClient;

    public ProductTypeService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task AddAsync(ProductType typeProduit)
    {
        await httpClient.PostAsJsonAsync<ProductType>("api/producttypes", typeProduit);
    }

    public async Task DeleteAsync(int id)
    {
        await httpClient.DeleteAsync($"api/producttypes/{id}");
    }

    public async Task<List<ProductType>?> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<ProductType>?>("api/producttypes");
    }

    public async Task<ProductType?> GetByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<ProductType?>($"api/producttypes/{id}");
    }
    public async Task UpdateAsync(ProductType updatedTypeProduct)
    {
        await httpClient.PutAsJsonAsync<ProductType>($"api/producttypes", updatedTypeProduct);
    }
}