using System.Text.Json.Serialization;
using JoinTheQueue.Core.Repository;
using JoinTheQueue.Core.Services;
using JoinTheQueue.Infrastructure.Database;
using JoinTheQueue.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace JoinTheQueue.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            });

            services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("v1", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header, // where to find apiKey, probably in a header
                    Name = "X-API-KEY", //header with api key
                    Type = SecuritySchemeType.ApiKey // this value is always "apiKey"
                });
            });


            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost*",
                            "http://localhost*");
                    });
            });

            //http clients
            services.AddHttpClient("WebHook", client => { });

            //Options Files
            // services.AddSingleton<FactOptions, FactOptions>(serviceProvider => Configuration.GetSection(nameof(FactOptions)).Get<FactOptions>());

            //DI - Core
            services.AddTransient<IManageServices, ManageServices>();
            services.AddTransient<IQueueServices, QueueServices>();
            //DI - Infrastructure
            services.AddTransient<IWebHookService, WebHookService>();
            services.AddTransient<IQueueDatabase, QueueDatabase>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Random Fact Slack Bot");
                c.RoutePrefix = string.Empty;
            });

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            app.UseResponseCompression();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}