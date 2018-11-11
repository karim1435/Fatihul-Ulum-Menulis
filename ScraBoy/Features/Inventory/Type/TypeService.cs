using ScraBoy.Features.Data;
using ScraBoy.Features.Type;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Inventory.Type
{
    public class TypeService
    {
        private readonly CMSContext  _db;
        public TypeService(CMSContext  context)
        {
            this._db = context;
        }
        public TypeService() :this(new CMSContext ()){ }
        public async Task<IEnumerable<TypeModel>> GetTypesAsync()
        {
            return await this._db.Type.OrderBy(c => c.TypeName).ToArrayAsync();
        }
    }
}