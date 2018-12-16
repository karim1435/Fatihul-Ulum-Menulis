using ScraBoy.Features.Data;
using ScraBoy.Features.Type;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Product
{
    public class ProductService
    {
        private readonly CMSContext  _db;
        public ProductService(CMSContext  context)
        {
            this._db = context;
        }
        public ProductService() :this(new CMSContext ()){ }

        public async Task<IEnumerable<ProductModel>> GetProductsByTypeAsync(string type)
        {
            return await _db.Product.Include("Type").Where(p => p.Type.TypeName.Equals(type)).ToArrayAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetProductsAsync()
        {
            return await _db.Product.OrderBy(a => a.ProductId).ToArrayAsync();
        }

        public async Task<ProductModel> GetProductByIdAync(int? id)
        {
            return await this._db.Product.Include("Type").Where(p => p.ProductId == id).FirstOrDefaultAsync();
        }

        public async Task AddProductAsync(ProductModel model)
        {
            this._db.Product.Add(model);

            await this._db.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(ProductModel model)
        {

            this._db.Entry(model).State = EntityState.Modified;

            await this._db.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var model = await GetProductByIdAync(id);
            this._db.Product.Remove(model);
            await this._db.SaveChangesAsync();
        }
    }
}