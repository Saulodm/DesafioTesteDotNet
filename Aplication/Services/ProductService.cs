using Core.Enums;
using Core.Interfaces;
using Core.Models;

namespace Aplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        private Product? InMemoryProduct;
        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Product> GetAll()
        {
            return _repo.GetAll().OrderByDescending(x => x.Name);
        }

        public Product GetById(Guid id)
        {
            InMemoryProduct = _repo.GetById(id);
            return InMemoryProduct;
        }

        public void Add(string name, decimal price, string category, int stockQauntity)
        {
            var item = new Product(name, price, Enum.Parse<Category>(category), stockQauntity);
            _repo.Add(item);
        }

        public void Remove(Guid id)
        {
            _repo.Remove(id);
        }

        public void Update(string name, decimal price, string category, int stockQauntity)
        {
            if (InMemoryProduct == null)
                return;
            InMemoryProduct.Name = name;
            InMemoryProduct.Price = price;
            InMemoryProduct.Category = Enum.Parse<Category>(category);
            InMemoryProduct.StockQuantity = stockQauntity;
            _repo.Update(InMemoryProduct);
            SetInMemoryProductNull();
        }

        public IEnumerable<string> GetCategories()
        {
            return Enum.GetNames(typeof(Category));
        }

        public void SetInMemoryProductNull()
        {
            InMemoryProduct = null;
        }
    }
}
