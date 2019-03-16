using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mq.Host.Data;
using Mq.Host.Handlers;
using Mq.Shared.Messages;
using Mq.Shared.Services;

namespace Mq.Host
{
    
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomServices(Configuration)
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            ConfigureMq(app);
            app.UseMvc();
        }

        private static void ConfigureMq(IApplicationBuilder app)
        {
            var mqService = app.ApplicationServices.GetRequiredService<IMessageQueueService>();
            mqService.Subscribe<CreateProductMessage, CreateProductResponseMessage, CreateProductBaseMessageHandler>();
            mqService.StartListening();
        }
    }
    
    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IProductContext, ProductContext>();
            services.AddSingleton<IMessageQueueService, MessageQueueService>();
            return services;
        }
    }
}
