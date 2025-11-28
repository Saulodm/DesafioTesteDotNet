using Aplication.Services;
using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using DesafioDotNet.Navigation;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioDotNet
{
    public static class Bootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Core ViewModels
            services.AddSingleton<ListProductViewModel>();
            services.AddTransient<AddProductViewModel>();
            // Navigation service (UI) - precisa estar registrado antes dos ViewModels que o consomem
            services.AddSingleton<INavigationService, NavigationService>();
            // EF Core InMemory via DbContextFactory (safe para singletons)
            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseInMemoryDatabase("DesafioDotNetDb"));
            // Forms (registrar AddProductForm como transient para criar nova instância a cada navegação)
            services.AddTransient<AddProductForm>();

            // Infrastructure
            services.AddSingleton<IRepository<Product>, ProductRepository>();

            // Services / Application
            services.AddSingleton<IProductService, ProductService>();

            // UI
            services.AddSingleton<ListProductsForm>();
        }
    }
}
