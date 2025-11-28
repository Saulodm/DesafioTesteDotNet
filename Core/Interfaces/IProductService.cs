using Core.Enums;
using Core.Models;

namespace Core.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        IEnumerable<string> GetCategories();
        void Add(string name, decimal price, string category, int stockQauntity);
        void Remove(Guid id);
        Product GetById(Guid id);
        void Update(string name, decimal price, string category, int stockQauntity);
        void SetInMemoryProductNull();
    }
}
