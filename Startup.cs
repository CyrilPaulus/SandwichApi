using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Converters;
using SandwichApi.Logic;
using SandwichApi.Models;
using System.IO;

namespace SandwichApi
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
            services.AddCors();
            services.AddTransient<UserLogic>();
            services.AddTransient<SandwichLogic>();
            services.AddTransient<TransactionLogic>();
            services.AddDbContext<SandwichContext>();
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.Converters.Add(new StringEnumConverter(

                    camelCaseText: true
                ));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder                    
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());


            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 &&
                   !Path.HasExtension(context.Request.Path.Value) &&
                   !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
