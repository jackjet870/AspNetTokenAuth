using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAuth
{
    public class Db : DbContext
    {
        public Db()
            : base("MyDb")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id)
                .Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id)
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithMany(r => r.Users)
                .Map(m =>
                {
                    m.ToTable("UsersInRoles");
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("Role_Id");
                });

            base.OnModelCreating(modelBuilder);
        }
    }

    public class User
    {
        public User()
        {
            UserRoles = new List<Role>();
        }

        public Guid Id { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }
        public virtual ICollection<Role> UserRoles { get; set; }
    }

    public class Role
    {
        public Role()
        {
            Users = new List<User>();
        }

        [Key]
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}