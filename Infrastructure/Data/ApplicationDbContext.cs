using Core.Models.Application;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
       
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


    }
}
