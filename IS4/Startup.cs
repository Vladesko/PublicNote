using IdentityServer4.Services;
using IS4.Entities;
using IS4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using IS4.ProfileService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IS4
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthorizeContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 6;
            }).
            AddEntityFrameworkStores<AuthorizeContext>().
            AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
               {
                   config.Authority = "https://localhost:7001";
                   config.Audience = "api";
               });

            services.AddAuthorization();

            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddIdentityServer().
                AddDeveloperSigningCredential().
                AddInMemoryClients(IS4.Configuration.GetClients()).
                AddInMemoryApiResources(IS4.Configuration.GetApiResources()).
                AddInMemoryApiScopes(IS4.Configuration.GetApiScopes()).
                AddInMemoryIdentityResources(IS4.Configuration.GetIdentityResources()).
                AddAspNetIdentity<AppUser>().
                AddProfileService<ProfileService.ProfileService>();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Notes.Identity.Cookie";
                config.LoginPath = "/Account/Login";
                config.LogoutPath = "/Account/Logout";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseIdentityServer();
            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
