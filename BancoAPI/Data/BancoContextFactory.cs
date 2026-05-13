using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BancoAPI.Data
{
    public class BancoContextFactory : IDesignTimeDbContextFactory<BancoContext>
    {
        public BancoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BancoContext>();
            optionsBuilder.UseMySql(
                "Server=localhost;Port=3306;Database=banco_db;User=root;Password=cimatec;AllowPublicKeyRetrieval=True;SslMode=None;",
                new MySqlServerVersion(new Version(8, 0, 0))
            );
            return new BancoContext(optionsBuilder.Options);
        }
    }
}
