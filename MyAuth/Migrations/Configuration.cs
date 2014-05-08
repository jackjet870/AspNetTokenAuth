using System.Collections.Generic;

namespace MyAuth.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyAuth.Db>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }

        protected override void Seed(MyAuth.Db context)
        {
            var role1 = new Role { Id = 1, RoleName = "admin" };
            var role2 = new Role { Id = 0, RoleName = "super admin" };

            context.Roles.AddOrUpdate(
                r => r.Id,
                role1,
                role2
            );
            var rAdmin = context.Roles.FirstOrDefault(r => r.RoleName == "admin");
            var rSuperAdmin = context.Roles.FirstOrDefault(r => r.RoleName == "super admin");

            var user1 = context.Users.Include(u => u.UserRoles).FirstOrDefault(u => u.UserName == "www");
            var user2 = context.Users.Include(u => u.UserRoles).FirstOrDefault(u => u.UserName == "www2");
            var user3 = context.Users.Include(u => u.UserRoles).FirstOrDefault(u => u.UserName == "www3");

            if (user1 == null && user2 == null && user3 == null)
            {
                user1 = new User { Id = Guid.NewGuid(), Password = "198921",UserName = "www", UserRoles = new List<Role> { rAdmin } };
                user2 = new User { Id = Guid.NewGuid(), Password = "198921", UserName = "www2", UserRoles = new List<Role> { rSuperAdmin } };
                user3 = new User { Id = Guid.NewGuid(), Password = "198921", UserName = "www3", UserRoles = new List<Role> { rSuperAdmin } };

                new List<User> 
                {
                    user1,
                    user2,
                    user3
                }.ForEach(u => context.Users.Add(u));
            }
            else
            {
                if (!user1.UserRoles.Any(r => r.Id == rAdmin.Id))
                    user1.UserRoles.Add(rAdmin);

                if (!user2.UserRoles.Any(r => r.Id == rSuperAdmin.Id))
                    user2.UserRoles.Add(rAdmin);

                if (!user3.UserRoles.Any(r => r.Id == rSuperAdmin.Id))
                    user3.UserRoles.Add(rAdmin);
            }
        }
    }
}
