using Core.Enums;
using Core.Interfaces;
using Core.Models;

namespace Aplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repo;

        private Product? InMemoryProduct;
        public ProductService(IRepository<Product> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Product> GetById(Guid id)
        {
            InMemoryProduct = await _repo.GetByIdAsync(id);
            return InMemoryProduct;
        }

        public async Task Add(string name, decimal price, string category, int stockQauntity)
        {
            var item = new Product(name, price, Enum.Parse<Category>(category), stockQauntity);
            await _repo.AddAsync(item);
        }

        public async Task Remove(Guid id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task Update(string name, decimal price, string category, int stockQauntity)
        {
            if (InMemoryProduct == null)
                return;
            InMemoryProduct.Name = name;
            InMemoryProduct.Price = price;
            InMemoryProduct.Category = Enum.Parse<Category>(category);
            InMemoryProduct.StockQuantity = stockQauntity;
            await _repo.UpdateAsync(InMemoryProduct);
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
