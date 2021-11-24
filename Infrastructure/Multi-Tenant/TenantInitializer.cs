using Core.Models.Tenants;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Multi_Tenant
{
    public class TenantInitializer : ITenantInitializer
    {
        private readonly IConfiguration _configuration;

        public TenantInitializer(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        /// <summary>
        /// Will seed the database for the given tenant.
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns>Will return the connection string for of generated database. If it fails will return null</returns>
        public string SeedDatabase(Tenants tenant)
        {
            try
            {
                string ConnectionString = _configuration.GetConnectionString("ConnectionStringTemplate");
                string TenantConnectionString = string.Format(ConnectionString, tenant.Name);
                var options = new DbContextOptions<ApplicationDbContext>();
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>(options);
                dbContextOptionsBuilder.UseSqlServer(TenantConnectionString);
                using (ApplicationDbContext db = new ApplicationDbContext(dbContextOptionsBuilder.Options))
                {
                    db.Database.Migrate();
                    
                };
                return TenantConnectionString;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}

