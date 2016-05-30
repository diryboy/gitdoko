using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gitdoko.Models
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext( DbContextOptions<AppDbContext> options ) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<CIResult> CIResults { get; set; }
    }
}
