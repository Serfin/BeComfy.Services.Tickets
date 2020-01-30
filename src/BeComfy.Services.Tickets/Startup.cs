using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BeComfy.Common.CqrsFlow;
using BeComfy.Common.EFCore;
using BeComfy.Common.Jaeger;
using BeComfy.Common.RabbitMq;
using BeComfy.Common.RestEase;
using BeComfy.Services.Tickets.EF;
using BeComfy.Services.Tickets.Messages.Commands;
using BeComfy.Services.Tickets.Messages.Events;
using BeComfy.Services.Tickets.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BeComfy.Services.Tickets
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();
            
            services.AddJaeger();
            services.AddOpenTracing();
            services.AddEFCoreContext<TicketsContext>();
            
            // Hardcoded addresses for now
            services.RegisterRestClientFor<IFlightsService>("http://localhost:5005");
            services.RegisterRestClientFor<ICustomersService>("http://localhost:5025");

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .AsImplementedInterfaces();
            builder.AddDispatcher();
            builder.AddHandlers();
            builder.AddRabbitMq();
            
            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseRabbitMq()
                .SubscribeCommand<BuyTicket>(
                    onError: (cmd, ex) => new BuyTicketRejected(cmd.Id, ex.Code, ex.Message));
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}