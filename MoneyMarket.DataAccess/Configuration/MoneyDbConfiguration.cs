using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace MoneyMarket.DataAccess.Configuration
{
    public class MoneyDbConfiguration : DbConfiguration
    {
        public MoneyDbConfiguration()
        {
            this.SetDatabaseInitializer(new CreateDatabaseIfNotExists<MoneyDbContext>());
            this.SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
        }
    }
}