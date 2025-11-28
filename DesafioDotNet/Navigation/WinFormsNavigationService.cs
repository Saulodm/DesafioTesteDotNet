using System.Reflection.Metadata;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioDotNet.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _provider;

        public NavigationService(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public void Show(string name)
        {
            switch (name)
            {
                case "AddProduct":
                    var second = _provider.GetService<AddProductForm>();
                    second?.Show();
                    break;
                default:
                    break;
            }
        }

        public bool? ShowDialog(string name, object? parameter = null)
        {
            switch (name)
            {
                case "AddProduct":
                    var product = _provider.GetService<AddProductForm>();
                    if (product is null) return null;
                    // se recebeu Guid como parâmetro, inicializa o form com o id
                    if (parameter is Guid guid)
                    {
                        product.Initialize(guid);
                    }
                    return product.ShowDialog() == DialogResult.OK;
                default:
                    return null;
            }
        }
    }
}
