using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gitdoko.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext( DbContextOptions<AppDbContext> options ) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<CIResult> CIResults { get; set; }

        protected override void OnModelCreating( ModelBuilder builder )
        {
            base.OnModelCreating(builder);

            builder.Entity<Issue>().HasBaseType<Topic>();
        }
    }
}
