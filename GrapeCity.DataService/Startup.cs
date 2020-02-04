using GrapeCity.DataService.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace GrapeCity.DataService
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
            services.AddMvcCore(options =>
            {
                options.EnableEndpointRouting = false;
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            services.AddDbContext<NorthwindContext>(options =>
                options
                .UseLazyLoadingProxies()
                .UseSqlServer(Configuration.GetConnectionString("NorthwindContext")));
            services.AddOData();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CA1822 // Mark members as static
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore CA1822 // Mark members as static
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

#pragma warning disable CA1062 // Validate arguments of public methods
            var builder = new ODataConventionModelBuilder(app.ApplicationServices);
#pragma warning restore CA1062 // Validate arguments of public methods
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Employee>("Employees");
            builder.EntitySet<Order>("Orders");
            builder.EntitySet<Territory>("Territories");
            builder.EntityType<OrderDetail>().HasKey(t => new { t.OrderId, t.ProductId });
            builder.EntitySet<OrderDetail>("OrderDetails");
            builder.EntitySet<Shipper>("Shippers");
            builder.EntitySet<Supplier>("Suppliers");
            builder.EntitySet<Region>("Regions");

            // app.UseHttpsRedirection();
            app.UseMvc(routeBuilder =>
            {
                routeBuilder.EnableDependencyInjection();

                // and this line to enable OData query option, for example $filter
                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

                routeBuilder.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());
            });
        }
    }
}
