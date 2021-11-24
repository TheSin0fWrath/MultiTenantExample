
using Core.Interfaces;
using Core.Models.Tenants;
using Infrastructure.Data;
using Infrastructure.Multi_Tenant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class TenantRepository :ITenant
    {
        private readonly TenantsDbContext _db;
        private readonly ITenantInitializer _tenant;

        public TenantRepository(TenantsDbContext db,ITenantInitializer _tenant)
        {
            _db = db;
            this._tenant = _tenant;
        }
        public async Task<bool> AddTenant(Tenants tenant)
        {

           tenant.ConnectionString= _tenant.SeedDatabase(tenant);
            _db.Tenants.Add(tenant);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<Tenants> GetTenantByName(string name) 
        {
          return await _db.Tenants.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }
        public string GetConnectionStringByName(string name)
        {
            return  _db.Tenants.Where(x => x.Name.Equals(name)).Select(x=>x.ConnectionString).FirstOrDefault();
        }

        public async Task<IEnumerable<Tenants>> GetAllTenants()
        {
            return await _db.Tenants.ToListAsync();
        }
    }
}
