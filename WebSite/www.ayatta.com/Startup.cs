using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ayatta.Web
{
    public class Startup
    {
        protected IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton(x => Configuration);
            //services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            //services.AddOptions();            
            services.Configure<StorageOptions>(Configuration.GetSection("ConnectionStrings"));


            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();

            services.AddDefaultStorage();

            services.AddSmsService();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Domain = "www.ikeepme.com";// WebSite.CookieDomain;
                    options.Cookie.Name = "x-auth";
                    options.Cookie.HttpOnly = true;
                    options.CookieDomain = "www.ikeepme.com";
                    options.Cookie.Path = "/";
                    
                    options.LoginPath = "/sign-in";
                    options.LogoutPath = "/sign-out";
                    options.AccessDeniedPath = "/account/denied";
                    options.ReturnUrlParameter = "redirect";
                });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie.Name = "x-xsrf-token";
                options.FormFieldName = "x-xsrf-token";
            });

            services.AddSession(options =>
            {
                options.Cookie.Name = "x-session";
            });
            services.AddCart();
            services.AddNsq();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }
    }
}