using App.DTO;
using App.Mapper;
using App.Models;
using App.Models.EntityFramework;
using App.Models.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddScoped<IDataRepository<Produit>, ProductManager>();
        builder.Services.AddScoped<IDataRepository<Marque>, MarqueManager>();
        builder.Services.AddScoped<IDataRepository<TypeProduit>, TypeProduitManager>();

        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("https://localhost:5002")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // DTO's Mapper configuration

        app.UseHttpsRedirection();

        // added manually
        app.UseCors("_myAllowSpecificOrigins");

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}