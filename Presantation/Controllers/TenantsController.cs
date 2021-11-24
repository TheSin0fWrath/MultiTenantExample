using Core.Interfaces;
using Core.Models.Tenants;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presantation.Controllers
{
    public class TenantsController : Controller
    {
        private readonly ITenant _tenant;

        public TenantsController(ITenant tenant)
        {
            _tenant = tenant;
        }
        public async Task<IActionResult> Index()
        {
            var user  = HttpContext.User.Claims;
           var result =await _tenant.GetAllTenants();
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Create(Tenants tenant)
        {
          var a =  await _tenant.AddTenant(tenant);
            return View();
        }
        [HttpGet]
        public  IActionResult GetBlogs(int id)
        {
            return RedirectToAction("index", "Blog", new { id });
        }
    }
}
