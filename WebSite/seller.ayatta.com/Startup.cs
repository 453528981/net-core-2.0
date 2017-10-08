using MediatR;
//using Ayatta.Event;
using System.Reflection;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            //services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            //services.AddMediatR(typeof(BaseEvent).GetTypeInfo().Assembly);


            services.Configure<StorageOptions>(Configuration.GetSection("ConnectionStrings"));

            services.AddDistributedMemoryCache();

            services.AddDefaultStorage();

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
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }
    }
}