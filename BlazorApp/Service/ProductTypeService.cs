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
        await httpClient.PostAsJsonAsync<ProductType>("api/typeproduit", typeProduit);
    }

    public async Task DeleteAsync(int id)
    {
        await httpClient.DeleteAsync($"api/typeproduit/{id}");
    }

    public async Task<List<ProductType>?> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<ProductType>?>("api/typeproduit");
    }

    public async Task<ProductType?> GetByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<ProductType?>($"api/typeproduit/{id}");
    }
    public async Task UpdateAsync(ProductType updatedTypeProduct)
    {
        await httpClient.PutAsJsonAsync<ProductType>($"api/typeproduit", updatedTypeProduct);
    }
}