using Core.Interfaces;
using Core.Models.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presantation.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IBlog _blog;

        public BlogController(IBlog blog)
        {
            _blog = blog;
        }
        public async Task<IActionResult> Index(int id)
        {
            IEnumerable<Blog> blogList = await _blog.GetAllBlogs();
            return View(blogList);
        }
        public async Task<IActionResult> Create(Blog newBlog)
        {
            var response = await _blog.AddBlog(newBlog);
            return RedirectToAction("Index");
        }
    }
}
