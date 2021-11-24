using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Application
{
    public class ApplicationUser: IdentityUser
    {
        public List<Blog> BogList { get; set; }
        [NotMapped]
        public string TenantId { get; set; }
    }
}
