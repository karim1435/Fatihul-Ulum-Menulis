using ScraBoy.Features.Data;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Shop
{
    public class ShopService
    {
        private readonly CMSContext  _db;
        public ShopService(CMSContext  context)
        {
            this._db = context;
        }
        public ShopService() :this(new CMSContext ()){ }
        public async Task<IEnumerable<ShopModel>> GetShopsAsync()
        {
            return await this._db.Shop.ToArrayAsync();
        }
    }
}