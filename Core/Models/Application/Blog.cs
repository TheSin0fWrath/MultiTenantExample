using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Application
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
