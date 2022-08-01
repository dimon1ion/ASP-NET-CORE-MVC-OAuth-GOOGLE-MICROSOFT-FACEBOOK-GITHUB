using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MW_ASP_NET_CORE_MVC_OAuth
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
            

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/index"; // Must be lowercase
                })
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                    microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                })
                .AddFacebook(options =>
                {
                    options.ClientId = Configuration["Authentication:Facebook:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Facebook:ClientSecret"];
                })
                .AddGitHub(o =>
                {
                    o.ClientId = Configuration["Authentication:GitHub:ClientId"];
                    o.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"];
                    o.CallbackPath = "/signin-github";
                });
            services.AddControllersWithViews();


        }

        //private OAuthOptions GitHubOptions =>
        //    new OAuthOptions()
        //    {
                
        //        ClientId = Configuration["GitHub:ClientId"],
        //        ClientSecret = Configuration["GitHub:ClientSecret"],
        //        CallbackPath = new PathString("/signin-github"),
        //        AuthorizationEndpoint = "https://github.com/login/oauth/authorize",
        //        TokenEndpoint = "https://github.com/login/oauth/access_token",
        //        UserInformationEndpoint = "https://api.github.com/user",
        //        ClaimsIssuer = "OAuth2-Github",

        //        //Retrieving user information is unique to each provider.
        //        Events = new OAuthEvents
        //        {
        //            OnCreatingTicket = async context => { await CreateGitHubAuthTicket(context); }
        //        }
        //    };

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
                    pattern: "{controller=Auth}/{action=Index}/{id?}");
            });
        }
    }
}
