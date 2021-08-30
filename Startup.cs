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
using CheckYourStocks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CheckYourStocks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionMoney = Configuration.GetConnectionString("DefaultConnection");
            string connectionUsers = Configuration.GetConnectionString("UsersConnection");
            services.AddDbContext<StockContext>(optiopn => optiopn.UseSqlServer(connectionMoney)
                                                                  .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                                                  );
            services.AddDbContext<DepositionContext>(optiopn => optiopn.UseSqlServer(connectionMoney)
                                                                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                                                );
            services.AddDbContext<UserContext>(optiopn => optiopn.UseSqlServer(connectionUsers)
                                                              .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                                              );
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                }
            );

            services.AddControllersWithViews();
        }
       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
