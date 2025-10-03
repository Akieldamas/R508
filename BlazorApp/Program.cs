using BlazorApp.Models;
using BlazorApp.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001/")
            });
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<MarqueService>();
            builder.Services.AddScoped<TypeProduitService>();
            builder.Services.AddScoped<ToastNotifications>();
            builder.Services.AddScoped<ProduitsTableViewModel>();
            builder.Services.AddScoped<MarqueViewModel>();
            builder.Services.AddScoped<TypeProduitViewModel>();
            await builder.Build().RunAsync();
        }
    }
}
