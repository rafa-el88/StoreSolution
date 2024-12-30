using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using StoreSolution.Server.Services;
using System.Reflection;
using StoreSolution.Core.Infraestructure.Context;

namespace StoreSolution.Server.Configuration
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StoreSolutionDbContext>
    {
        public StoreSolutionDbContext CreateDbContext(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<StoreSolutionDbContext>();
            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            builder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly(migrationsAssembly));
            builder.UseOpenIddict();

            return new StoreSolutionDbContext(builder.Options, SystemUserIdAccessor.GetNewAccessor());
        }
    }
}
