using Aplication.Services;
using Aplication.Services.Export;
using Core.Interfaces;
using Core.Models;
using Core.Reports;
using Core.ServiceBus;
using Core.ViewModels;
using DesafioDotNet.Navigation;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DesafioDotNet
{
    public static class Bootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Logging
            services.AddLogging(config => config.SetMinimumLevel(LogLevel.Information));
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
            services.AddTransient<ListProductsForm>();

            // Infrastructure
            services.AddSingleton<IRepository<Product>, ProductRepository>();

            // Services / Application
            services.AddSingleton<IProductService, ProductService>();
            // Exporter + higher level export service
            services.AddSingleton<ICsvExporter, CsvExporter>();
            services.AddSingleton<IProductExportService, ProductExportService>();

            // ServiceBus generic registration example
            services.AddSingleton(typeof(IServiceBus<>), typeof(InMemoryServiceBus<>));

            // UI
            services.AddSingleton<ListProductsForm>();
        }
    }
}
