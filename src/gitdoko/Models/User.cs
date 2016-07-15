using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace gitdoko.Models
{
    public class User : IdentityUser
    {
        public ICollection<Project> Projects { get; set; }
        public ICollection<Topic> Topics { get; set; }
        public ICollection<Discussion> Discussions { get; set; }
    }
}
