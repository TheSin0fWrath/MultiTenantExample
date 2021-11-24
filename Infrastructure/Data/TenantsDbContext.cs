using Core.Models.Tenants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class TenantsDbContext:DbContext
    {
        public TenantsDbContext(DbContextOptions<TenantsDbContext> options) :base(options)
        {

        }
        public DbSet<Tenants> Tenants { get; set; }
    }
}
