using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Multi_Tenant
{
    public interface ITenantResolver<out TService>
    {
        TService Resolve();
    }

    public class TenantConnectionStringResolver : ITenantResolver<string>
    {
        private readonly ClaimsPrincipal _user;
        private readonly ITenant _configuration;

        public TenantConnectionStringResolver(ClaimsPrincipal user, ITenant configuration)
        {
            _user = user;
            _configuration = configuration;
        }

        public string Resolve()
        {
            var tenantId = _user.Claims.SingleOrDefault(c => c.Type == "TenantName");
            var connectionString = _configuration.GetConnectionStringByName(tenantId.Value);
            return connectionString;
        }
    }
}
