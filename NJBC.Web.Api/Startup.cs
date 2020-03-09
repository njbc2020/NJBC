using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using NJBC.DataLayer.Models;
using NJBC.DataLayer.IRepository;
using NJBC.DataLayer.Repository;

namespace NJBC.Web.Api
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
            services.AddControllers();

            services.AddDbContext<NJBC_DBContext>(x =>
            x.UseSqlServer("Data source=.;initial catalog=NJBC_DB;user id=sa;password=123;MultipleActiveResultSets=True;"));


            services.AddScoped<ISemEvalRepository, SemEvalRepository>();
            //services.AddScoped< ISemEvalService, SemEvalService>();
            //services.AddDbContext<AppilcationDbContext>(options =>
            //{
            //    //options.UseSqlServer(
            //    //    Configuration.GetConnectionString("DefaultConnection")
            //    //                .Replace("|DataDirectory|", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app_data")),
            //    //    serverDbContextOptionsBuilder =>
            //    //    {
            //    //        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
            //    //        serverDbContextOptionsBuilder.CommandTimeout(minutes);
            //    //        serverDbContextOptionsBuilder.EnableRetryOnFailure();
            //    //    });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
