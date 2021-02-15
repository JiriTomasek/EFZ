using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EFZ.Core.Database.DbContextFactory
{
    public abstract class AbstractDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext

    {
        #region constructor

        protected AbstractDbContextFactory()
        {
        }


        #endregion

        public TContext Create()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName);
        }

        private TContext Create(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder().SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true);

            builder.AddEnvironmentVariables();
            var config = builder.Build();


            var connectionStringName = GetConnStringName("EfzToolDb");

            var connStr = config.GetConnectionString(connectionStringName);



            if (String.IsNullOrWhiteSpace(connStr) == true)
            {

                throw new InvalidOperationException(
                    $"Could not find a connection string named '{connectionStringName}'.");
            }
            else
            {
                return CreateContext(connStr);
            }
        }
        
        public static string GetConnStringName(string defaultConnStringName)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var basePath = AppContext.BaseDirectory;
            var builder = new ConfigurationBuilder().SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true);

            builder.AddEnvironmentVariables();
            var config = builder.Build();


            return string.IsNullOrEmpty(GetConnStringName(config)) ? defaultConnStringName : GetConnStringName(config);
        }

        private static string GetConnStringName(IConfigurationRoot config)
        {
            return config.GetValue<string>("ConnectionStringName");
        }

        private TContext CreateContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            Console.WriteLine(
                "DesignTimeDbContextFactoryBase.Create(string): Connection string: {0}",
                connectionString);

            optionsBuilder.UseSqlServer(connectionString);

            DbContextOptions<TContext> options = optionsBuilder.Options;

            return CreateNewInstance(options);
        }

        public TContext CreateDbContext(string[] args)
        {
            return Create(
                Directory.GetCurrentDirectory(),
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);
    }
}
