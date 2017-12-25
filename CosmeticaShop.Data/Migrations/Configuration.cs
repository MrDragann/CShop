using CosmeticaShop.Data.Models;

namespace CosmeticaShop.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //context.Roles.AddOrUpdate(
            //      p => p.Id,
            //      new Role { Id = 1, Name = "User" },
            //      new Role { Id = 2, Name = "Admin" }
            //    );

            //context.SitePages.AddOrUpdate(
            //      p => p.Id,
            //      new SitePage { Id = 1, Title = "CosmeticaShop.ro" }
            //    );
            if (!context.Cities.Any())
            {
                context.Cities.AddOrUpdate(new City { Id = 1, Name = "București" });
                context.Cities.AddOrUpdate(new City { Id = 2, Name = "Brașov" });
                context.Cities.AddOrUpdate(new City { Id = 3, Name = "Timișoara" });
            }
        }
    }
}
