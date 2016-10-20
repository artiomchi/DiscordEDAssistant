using FlexLabs.EDAssistant.Injection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace FlexLabs.EDAssistant.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.secrets.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables(Models.Settings.EnvironmentPrefix);
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<Models.Settings>(Configuration);

            var dbConnectionString = Configuration.GetConnectionString("DefaultConnection");
            ServiceMappings.ConfigureDatabase(services, dbConnectionString);
            ServiceMappings.ConfigureServices(services);

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Setting the Microsoft Bot Framework credentials
                var botFrameworkSettings = serviceProvider.GetService<IOptions<Models.Settings>>().Value.BotFramework;
                var appSettings = System.Configuration.ConfigurationManager.AppSettings;
                appSettings["BotId"] = botFrameworkSettings.BotId;
                appSettings["MicrosoftAppId"] = botFrameworkSettings.MicrosoftAppId;
                appSettings["MicrosoftAppPassword"] = botFrameworkSettings.MicrosoftAppPassword;
            }

            var dbConnectionString = Configuration.GetConnectionString("DefaultConnection");
            ServiceMappings.InitDatabase(dbConnectionString);

            app.UseMvc();

            app.Use((context, next) => context.Response.WriteAsync(""));
        }
    }
}
