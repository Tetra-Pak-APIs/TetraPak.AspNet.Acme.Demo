using demo.AcmeProducts.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TetraPak.AspNet;
using TetraPak.AspNet.Api;
using TetraPak.AspNet.Api.Auth;
using TetraPak.AspNet.Auth;

namespace demo.AcmeProducts
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "demo.Api.AcmeProducts", Version = "v1" });
            });
            services.AddTetraPakJwtBearerAssertion();
            services.AddRepositories();
            services.AddTetraPakServices();              // <-- add this _after_ services.AddControllers() to support backend Tetra Pak services
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "demo.Api.AcmeProducts v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseTetraPakClientAuthentication(env);

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            app.LogTetraPakConfiguration(LogLevel.Information);
        }
    }
}