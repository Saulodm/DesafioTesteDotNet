using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly AppDbContext _dbContext;

        public ProductRepository(IDbContextFactory<AppDbContext> factory)
        {
            _dbContext = factory.CreateDbContext();
        }
        public void Add(Product item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            _dbContext.Products.Add(item);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            return _dbContext.Products.AsNoTracking().ToList();
        }

        public Product GetById(Guid id)
        {
            return _dbContext.Products.Where(c => c.Id == id).FirstOrDefault();
        }

        public void Remove(Guid id)
        {
            var e = _dbContext.Products.Find(id);
            if (e != null)
            {
                _dbContext.Products.Remove(e);
                _dbContext.SaveChanges();
            }
        }

        public void Update(Product item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _dbContext.Products.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
