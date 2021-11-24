using Core.Models.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITenant
    {
        Task<IEnumerable<Tenants>> GetAllTenants();
        Task<bool> AddTenant(Tenants tenant);
        Task<Tenants> GetTenantByName(string name);
        string GetConnectionStringByName(string name);

    }
}
