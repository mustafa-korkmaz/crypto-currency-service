using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MoneyMarket.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MoneyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MoneyDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //to avoid creating duplicate seed data. E.g.

            /*
            role Admin = "ede64fc4-7b30-45f7-b92d-e71f3d0027b6"
            role Owner = "911f5c44-c2d1-43aa-9247-ec71fbc17956"
            */

            context.Roles.AddOrUpdate(
                p => new { p.Name, p.Id },
                new IdentityRole
                {
                    Name = "Admin",
                    Id = "ede64fc4-7b30-45f7-b92d-e71f3d0027b6"
                },
                new IdentityRole
                {
                    Name = "Owner",
                    Id = "911f5c44-c2d1-43aa-9247-ec71fbc17956"
                });
        }
    }
}
