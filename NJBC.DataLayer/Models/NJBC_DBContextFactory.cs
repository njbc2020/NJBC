using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace NJBC.DataLayer.Models
{
    public class NJBC_DBContextFactory : IDesignTimeDbContextFactory<NJBC_DBContext>
    {
        public NJBC_DBContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            Console.WriteLine($"Using `{basePath}` as the BasePath");
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(basePath)
                                    .AddJsonFile("appsettings.json")
                                    .Build();
            var builder = new DbContextOptionsBuilder<NJBC_DBContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                                .Replace("|DataDirectory|", Path.Combine(basePath, "wwwroot", "app_data"));
            builder.UseSqlServer(connectionString);
            return new NJBC_DBContext(builder.Options);
        }
    }
}
