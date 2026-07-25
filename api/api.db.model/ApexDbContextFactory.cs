using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Djambi.Api.Db.Model
{
    public class DjambiDbContextFactory : IDesignTimeDbContextFactory<DjambiDbContext>
    {
        public DjambiDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables("DJAMBI_");

            var config = builder.Build();

            var connStr = config.GetSection("Sql:ConnectionString").Value;
            var version = ServerVersion.AutoDetect(connStr);

            var optionsBuilder = new DbContextOptionsBuilder<DjambiDbContext>();
            optionsBuilder.UseMySql(connStr, version);

            return new DjambiDbContext(optionsBuilder.Options);
        }
    }
}
