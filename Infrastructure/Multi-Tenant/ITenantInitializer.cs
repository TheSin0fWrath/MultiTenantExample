using Core.Models.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Multi_Tenant
{
    public interface ITenantInitializer
    {
       string SeedDatabase(Tenants tenants);
    }
}
