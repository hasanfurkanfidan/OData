using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ODataLearning.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using ODataLearning.Api.Entities;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using ODataLearning.Api.Models;

namespace ODataLearning.Api
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

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });
            services.AddOData();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ODataLearning.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Product>("Products");

            builder.EntityType<Category>().Action("TotalProductPrice").Returns<decimal>();
            builder.EntityType<Category>().Collection.Action("TotalProductPrice2").Returns<decimal>();

            builder.EntityType<Category>().Collection.Action("TotalProductPriceWithParameter").Returns<decimal>().Parameter<int>("categoryId");
            builder.EntityType<Product>().Collection.Action("Login").Returns<string>().Parameter<Login>("UserLogin");
            var actionTotal = builder.EntityType<Category>().Collection.Action("Total").Returns<int>();
            actionTotal.Parameter<int>("a");
            actionTotal.Parameter<int>("b");
            actionTotal.Parameter<int>("c");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ODataLearning.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseODataBatching();


            app.UseEndpoints(endpoints =>
            {
                endpoints.Select().Expand().OrderBy().MaxTop(null).SkipToken().Count().Filter();
                endpoints.MapODataRoute("odata", "odata", builder.GetEdmModel());
                endpoints.MapControllers();
                
            });
        }
    }
}
