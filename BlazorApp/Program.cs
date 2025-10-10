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
                BaseAddress = new Uri("http://localhost:5001/")
            });
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<BrandService>();
            builder.Services.AddScoped<ProductTypeService>();
            builder.Services.AddScoped<ToastNotifications>();
            builder.Services.AddScoped<ProductsViewModel>();
            builder.Services.AddScoped<BrandViewModel>();
            builder.Services.AddScoped<ProductTypeViewModel>();
            await builder.Build().RunAsync();
        }
    }
}
