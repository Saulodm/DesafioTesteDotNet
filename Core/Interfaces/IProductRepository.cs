using Core.Models;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        void Add(Product item);
        void Remove(Guid id);
        void Update(Product item);
        Product GetById(Guid id);
    }
}
