using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Brighter.WebApp.Data;
using Brighter.WebApp.Models;
using Brighter.WebApp.Services;
//using Paramore.Darker.AspNetCore;
//using Paramore.Darker.Policies;
//using Paramore.Darker.QueryLogging;

using Polly;
//using Account.Models.Queries;
//using Account.Services.Handlers;
//using Account.Models;
using Features.Account;
//using Paramore.Brighter.AspNetCore;
using MediatR;

namespace Brighter.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMediatR(
                 typeof(PasswordSignInQuery).Assembly,
                typeof(PasswordSignInQueryHandler).Assembly
                );

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            //services.AddBrighter().AsyncHandlersFromAssemblies(
            //    typeof(PasswordSignInQuery).Assembly,
            //    typeof(PasswordSignInQueryHandlerAsync).Assembly
            //    )
            //    ;

            //services.AddDarker()
            //    //.AddHandlers(registry =>
            //    //{
            //    //    registry.Register(typeof(AuthenticateQuery), typeof(AuthenticateQuery.Result), typeof(AuthenticateQueryHandlerAsync));
            //    //})
            //    .AddHandlersFromAssemblies(
            //    typeof(PasswordSignInQuery).Assembly,
            //    typeof(PasswordSignInQueryHandlerAsync).Assembly
            //    )
            //    //.AddJsonQueryLogging()
            //    //.AddPolicies(ConfigurePolicies())
            //    ;
           

            //services.AddSingleton<IQueryProcessor, QueryProcessor>();




            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //private IPolicyRegistry ConfigurePolicies()
        //{
        //    var defaultRetryPolicy = Policy
        //        .Handle<Exception>()
        //        .WaitAndRetryAsync(new[]
        //        {
        //            TimeSpan.FromMilliseconds(50),
        //            TimeSpan.FromMilliseconds(100),
        //            TimeSpan.FromMilliseconds(150)
        //        });

        //    var circuitBreakerPolicy = Policy
        //        .Handle<Exception>()
        //        .CircuitBreakerAsync(1, TimeSpan.FromMilliseconds(500));

        //    var circuitBreakTheWorstCaseScenario = Policy
        //        .Handle<SomethingWentTerriblyWrongException>()
        //        .CircuitBreakerAsync(1, TimeSpan.FromSeconds(5));

        //    return new PolicyRegistry
        //    {
        //        //{ Paramore.Darker.Policies.Constants.RetryPolicyName, defaultRetryPolicy },
        //        //{ Paramore.Darker.Policies.Constants.CircuitBreakerPolicyName, circuitBreakerPolicy },
        //        //{ SomethingWentTerriblyWrongCircuitBreakerName, circuitBreakTheWorstCaseScenario }
        //        { Paramore.Darker.Policies.Constants.RetryPolicyName, defaultRetryPolicy },
        //        { Paramore.Darker.Policies.Constants.CircuitBreakerPolicyName, circuitBreakerPolicy },
        //        { SomethingWentTerriblyWrongCircuitBreakerName, circuitBreakTheWorstCaseScenario }
        //    };
        //}

        //internal const string SomethingWentTerriblyWrongCircuitBreakerName = "SomethingWentTerriblyWrongCircuitBreaker";
    }
}
