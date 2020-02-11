using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BeComfy.Common.Consul;
using BeComfy.Common.CqrsFlow;
using BeComfy.Common.EFCore;
using BeComfy.Common.Jaeger;
using BeComfy.Common.Mongo;
using BeComfy.Common.RabbitMq;
using BeComfy.Common.RestEase;
using BeComfy.Services.Tickets.Domain;
using BeComfy.Services.Tickets.EF;
using BeComfy.Services.Tickets.Messages.Commands;
using BeComfy.Services.Tickets.Messages.Events;
using BeComfy.Services.Tickets.Services;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddConsul();
            services.AddMongo();
            services.AddMongoRepository<Ticket>("Tickets");
            services.AddEFCoreContext<TicketsContext>();
            
            // Hardcoded addresses for now
            services.RegisterRestClientFor<IFlightsService>("becomfy-services-flights");
            services.RegisterRestClientFor<ICustomersService>("becomfy-services-customers");

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConsulClient consulClient,
            IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseRabbitMq()
                .SubscribeCommand<BuyTicket>(
                    onError: (cmd, ex) => new BuyTicketRejected(cmd.Id, cmd.CustomerId, ex.Code, ex.Message));
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var consulServiceId = app.UseConsul();
            applicationLifetime.ApplicationStopped.Register(
                () => consulClient.Agent.ServiceDeregister(consulServiceId)
            );

        }
    }
}