using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace spa_stack
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNoDbStorageForToDoItems();

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddMvc()
                //.AddApplicationPart(Assembly.Load(new AssemblyName("WebLib")))
                .AddApplicationPart(Assembly.Load(new AssemblyName("CSharp.Web")))
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //https://docs.asp.net/en/latest/fundamentals/static-files.html

            // Serve my app-specific default file, if present.
            var defaultFileOptions = new DefaultFilesOptions();
            defaultFileOptions.DefaultFileNames.Clear();
            // I specifically chose a non tradiditional default file name here
            // because I'm actually envisioning that api apps may be added in with
            // MVC handling the default view with links to the static html app pages from the
            // MVC app, in which case this default document would be removed
            // this means I don't have to rename my app html files later and the names don't imply
            // a default document
            defaultFileOptions.DefaultFileNames.Add("myapp.html");
            app.UseDefaultFiles(defaultFileOptions);
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                // seems for the F# controller the default route doesn't find it so need to explicitely add it
                //routes.MapRoute(
                //    name: "ToDo",
                //    template: "ToDo/{action=Index}/{id?}");

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
