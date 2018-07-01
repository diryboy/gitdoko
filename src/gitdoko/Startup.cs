using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace gitdoko
{
    public class Startup
    {
        private readonly IHostingEnvironment HostingEnvironment;

        public static void Main( string[] args )
        {
            var host = new WebHostBuilder();

            host.UseKestrel()
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory()) //???
                .UseStartup<Startup>()
                ;

            host.Build().Run();
        }

        public Startup( IHostingEnvironment env )
        {
            HostingEnvironment = env;
        }

        // Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services )
        {
            var dbPath = Path.Combine(HostingEnvironment.ContentRootPath, @"AppData\gitdoko.db");
            services//.AddSqliteDatabase()
                    .AddDbContext<AppDbContext>(db => db.UseSqlite($"Data Source={dbPath}"))
                    ;

            var idServices = services.AddIdentity<User, IdentityRole>(id =>
            {
                id.User.RequireUniqueEmail = false;
            });

            idServices.AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/SignIn");

            services.AddMvc();
        }

        // Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, AppDbContext dbContext )
        {
            if ( HostingEnvironment.IsDevelopment() )
            {
                dbContext.Database.EnsureCreated();
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseFileServer()
               .UseMvc()
               ;
        }
    }
}
