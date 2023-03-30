using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _cotnext;

        public ProductRepository(StoreContext cotnext)
        {
            _cotnext = cotnext;
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _cotnext.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _cotnext.Products.ToListAsync();
        }
    }

   
}
