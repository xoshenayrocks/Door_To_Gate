using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DoorToGate.Services;
using Identity.Dapper;
using Identity.Dapper.SqlServer.Connections;
using Identity.Dapper.Models;
using Identity.Dapper.Entities;
using Identity.Dapper.SqlServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Door_To_Gate.Services;

namespace DoorToGate
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);


            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            Configuration = configuration;
        
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IAirportCodeClient, AirportCodeClient>(client => client.BaseAddress = new Uri("http://api.aviationstack.com/v1/"));
            services.AddHttpClient<ITSAClient, TSAClient>(client => client.BaseAddress = new Uri("https://www.tsawaittimes.com/api/airport/nqGl3FjKwukKYh9yn1daCVzWCtEU1s98/"));
            services.ConfigureDapperConnectionProvider<SqlServerConnectionProvider>(
                Configuration.GetSection("ConnectionStrings")
            ).ConfigureDapperIdentityCryptography(Configuration.GetSection("DapperIdentityCryptography"))
             .ConfigureDapperIdentityOptions(new DapperIdentityOptions { UseTransactionalBehavior = false });

            services.AddIdentity<DapperIdentityUser, DapperIdentityRole>(identityOptions =>
            {
                identityOptions.Password.RequireDigit = false;
                identityOptions.Password.RequiredLength = 1;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
            })
            .AddDapperIdentityFor<SqlServerConfiguration>()
            .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
