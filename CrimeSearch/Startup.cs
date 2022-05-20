using CrimeSearch.Interfaces;
using CrimeSearch.Models;
using CrimeSearch.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CrimeSearch
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
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            var dbSettings = new DBSettings();
            dbSettings.DatabaseName = "test";
            dbSettings.CollectionName = "CrimeInstance";
            dbSettings.ConnectionString = "*";

            var mongoClient = new MongoClient(dbSettings.ConnectionString);

            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(dbSettings.DatabaseName);

            IMongoCollection<JObject> crimeCollection = mongoDatabase.GetCollection<JObject>(dbSettings.CollectionName);
            IMongoCollection<Dictionary<string, object>> collection = mongoDatabase.GetCollection<Dictionary<string, object>>(dbSettings.CollectionName);

            services.AddSingleton(mongoDatabase);
            services.AddSingleton(crimeCollection);
            services.AddSingleton(collection);
            services.AddSingleton(dbSettings);
            //services.AddTransient<DynamicSearchService, DynamicSearchService>();
            services.AddTransient<CrimeSearchService, CrimeSearchService>();
            services.AddTransient<IPredicateOperationBuilder, PredicateOperationBuilder>();
            services.AddTransient<ExpressionBuilder, ExpressionBuilder>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
