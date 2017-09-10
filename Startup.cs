using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration; //*--Added for secure DB use
using Microsoft.AspNetCore.Hosting;

namespace movie_API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddMvc();
            services.Configure<MySqlOptions>(Configuration.GetSection("DBInfo")); //*--Added for secure DB use
            services.AddScoped<DbConnector>();
            services.Configure<APIKeyOptions>(Configuration.GetSection("APIKey")); //*--Added to secure API key
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }
        //*--Added for secure DB use
        public IConfiguration Configuration { get; private set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
    }
}
