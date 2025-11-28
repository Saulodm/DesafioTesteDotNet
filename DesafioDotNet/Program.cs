using Microsoft.Extensions.DependencyInjection;

namespace DesafioDotNet
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var services = new ServiceCollection();
            Bootstrapper.RegisterServices(services);
            using var provider = services.BuildServiceProvider();

            var mainForm = provider.GetRequiredService<ListProductsForm>();
            Application.Run(mainForm);
        }
    }
}