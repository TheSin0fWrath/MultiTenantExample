using Core.Interfaces;
using Core.Models.Application;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class BlogRepository : IBlog
    {
        private readonly ApplicationDbContext _db;

        public BlogRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddBlog(Blog blog)
        {
           _db.Blogs.Add(blog);
            await  _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Blog>> GetAllBlogs()
        {
            return await  _db.Blogs.ToListAsync();  
        }
    }
}
