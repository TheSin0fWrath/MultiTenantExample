using Core.Interfaces;
using Core.Models.Application;
using Infrastructure.Data;
using Infrastructure.Multi_Tenant;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presantation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<IBlog, BlogRepository>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<TenantsDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("TenantConnectionString")));
            services.AddRazorPages();

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders()
              .AddDefaultUI();
           // services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,
            //AdditionalUserClaimsPrincipalFactory>();
            services.AddScoped<ITenantResolver<string>>(svc =>
            {
                var accessor = svc.GetRequiredService<IHttpContextAccessor>();
                var context = accessor.HttpContext;
                var user = context.User;
                var configuration = svc.GetRequiredService<ITenant>();
                return new TenantConnectionStringResolver(user, configuration);
            });

            services.AddDbContext<ApplicationDbContext>((svc, options) =>
            {
                var tenantResolver = svc.GetRequiredService<ITenantResolver<string>>();
                options.UseSqlServer(tenantResolver.Resolve());
            });
            services.AddScoped<ITenant, TenantRepository>();
            services.AddScoped<ITenantInitializer, TenantInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

            });
        }
    }
}
