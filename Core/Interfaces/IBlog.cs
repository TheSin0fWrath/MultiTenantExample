using Core.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBlog
    {
        Task<IEnumerable<Blog>> GetAllBlogs();
        Task<bool> AddBlog(Blog blog);

    }
}
