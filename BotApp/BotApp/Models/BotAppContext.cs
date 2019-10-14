using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BotApp.Models
{
    public class BotAppContext : DbContext
    {
        public BotAppContext() : base("BotAppContext")
        { }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }

    public class UserDbInitializer : DropCreateDatabaseAlways<BotAppContext>
    {
        protected override void Seed(BotAppContext db)
        {
            db.Roles.Add(new Role { Id = 1, Name = "admin" });
            db.Roles.Add(new Role { Id = 2, Name = "user" });
            db.Users.Add(new User
            {
                Id = 1,
                Name = "Banner",
                Password = "Off118",
                Login = "Off118",
                RoleId = 1      
            });
            base.Seed(db); 
        }
    }
}