using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IRepository<Product>
    {

        private readonly AppDbContext _dbContext;

        public ProductRepository(IDbContextFactory<AppDbContext> factory)
        {
            _dbContext = factory.CreateDbContext();
        }
    
        public async Task AddAsync(Product entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbContext.Products.Add(entity);
            _dbContext.SaveChanges();
        }

        public async Task DeleteAsync(Guid id)
        {
            var e = _dbContext.Products.Find(id);
            if (e != null)
            {
                _dbContext.Products.Remove(e);
                _dbContext.SaveChanges();
            }
        }

      
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return _dbContext.Products.AsNoTracking().ToList();
        }


        public async Task<Product> GetByIdAsync(Guid id)
        {
            return _dbContext.Products.Where(c => c.Id == id).FirstOrDefault();
        }

   
        public async Task UpdateAsync(Product entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbContext.Products.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
