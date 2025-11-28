using Core.Enums;
using Core.Models;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        IEnumerable<string> GetCategories();
        Task Add(string name, decimal price, string category, int stockQauntity);
        Task Remove(Guid id);
        Task<Product> GetById(Guid id);
        Task Update(string name, decimal price, string category, int stockQauntity);
        void SetInMemoryProductNull();
    }
}
