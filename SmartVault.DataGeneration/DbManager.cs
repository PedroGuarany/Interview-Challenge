using Microsoft.Extensions.Configuration;
using System.Data.SQLite;
using System.IO;


namespace SmartVault.Program.QueryLayer
{
    public class DbManager
    {
        private readonly string config;
        public DbManager()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            config = string.Format(configuration?["ConnectionStrings:DefaultConnection"] ?? "", configuration?["DatabaseFileName"]);
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(config);
        }
    }
}
