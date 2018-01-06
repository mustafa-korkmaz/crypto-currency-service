using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Identity.EntityFramework;
using MoneyMarket.DataAccess.Configuration;
using MoneyMarket.DataAccess.Models;

namespace MoneyMarket.DataAccess
{
    public class MoneyDbContext : IdentityDbContext<ApplicationUser>
    {
        static MoneyDbContext()
        {
            Database.SetInitializer(new MoneyDblInitializer());
        }

        public MoneyDbContext()
            : base("MoneyDbConnection")
        {
#if DEBUG
            this.Database.Log = LogQuery; // print sql in debug mode
#endif

            this.Configuration.UseDatabaseNullSemantics = true; // to avoid unneccessary null checks
        }

        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<CryptoCurrencyException> Exceptions { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<TeamScope> TeamRole { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<TeamCryptoCurrencyBalance> TeamCryptoCurrencyBalances { get; set; }
        public DbSet<TeamNotification> TeamNotifications { get; set; }

        public static MoneyDbContext Create()
        {
            return new MoneyDbContext();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : EntityBase
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
           .Where(type => !string.IsNullOrEmpty(type.Namespace))
           .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            modelBuilder.Properties<string>().Configure(x => x.HasColumnType("VARCHAR")); // dont use unicode for string columns
            modelBuilder.Properties<DateTime>().Configure(x => x.HasColumnType("datetime2").HasPrecision(0)); // dont need miliseconds for datetime columns
            modelBuilder.Entity<CryptoCurrency>().Property(x => x.UsdValue).HasPrecision(18, 6);
            modelBuilder.Entity<TeamCryptoCurrencyBalance>().Property(x => x.Balance).HasPrecision(18, 6);

            base.OnModelCreating(modelBuilder);
        }

        private static void LogQuery(string sql)
        {
            // This text is always added, making the file longer over time  if it is not deleted.
            using (StreamWriter sw = File.AppendText(@"D:\\QueryLogs_CryptoCurrency.sql"))
            {
                if (sql.Length > 5)
                    if (sql.Substring(4, 2).ToLower() == "ed") // opened or closed db string
                    {
                        sql = "-- " + sql;
                    }
                sw.WriteLine(sql);
            }
        }

    }
}

