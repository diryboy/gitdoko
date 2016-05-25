using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace gitdoko
{
    public class Startup
    {
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

        // Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddMvc();
        }

        // Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = "/SignIn",
                ReturnUrlParameter = "ReturnUrl",
            });

            app.UseFileServer()
               .UseMvc(ConfigureRoutes)
               ;
        }

        private static void ConfigureRoutes( IRouteBuilder routes )
        {
            routes.MapRoute("default", "{controller}/{action=Index}/{name?}")
                  .MapRoute("special", "{action=Index}/{name?}", new { controller = "Home" })
                  ;
        }
    }
}
