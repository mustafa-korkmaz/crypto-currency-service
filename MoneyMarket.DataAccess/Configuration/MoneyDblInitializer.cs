using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MoneyMarket.DataAccess.Configuration
{
    public class MoneyDblInitializer : IDatabaseInitializer<MoneyDbContext>
    {
        public void InitializeDatabase(MoneyDbContext context)
        {
            if (!context.Database.Exists())
            {
                // if database did not exist before - create it
                context.Database.Create();
            }
            else
            {
                //when migration runs successfully for the first time, we dont need this code block anymore
                //query to check if MigrationHistory table is present in the database
                var migrationHistoryTableExists = ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>(
                string.Format(
                  "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' AND table_name = '__MigrationHistory'",
                  "betblog"));

                // if MigrationHistory table is not there (which is the case first time we run) - create it
                if (migrationHistoryTableExists.FirstOrDefault() == 0)
                {
                    context.Database.Delete();
                    context.Database.Create();
                }
            }
        }
    }
}