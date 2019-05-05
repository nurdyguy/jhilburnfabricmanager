using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JHilburnFabricManager.Services.Contracts;
using JHilburnFabricManager.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JHilburnFabricManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            //Mappings.AutoMapperConfig.Configure();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(cookieOptions =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                cookieOptions.CheckConsentNeeded = context => true;
                cookieOptions.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            Action<JFMOptions> options = (opt =>
            {
                opt.FabricApi.BaseUrl = Configuration["ApiProperties:FabricApi:BaseUrl"];
                var apiTokens = Configuration.GetSection("ApiProperties:FabricApi:Tokens").Get<Dictionary<string, string>>();
                foreach(var token in apiTokens)
                {
                    opt.FabricApi.Tokens.Add(token.Key, token.Value);
                }

            });
            services.Configure(options);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JFMOptions>>().Value);

            services.AddSingleton<IFabricDataService, FabricDataService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
