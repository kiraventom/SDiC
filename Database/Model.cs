using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationDB
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=authorization.db");
    }

    public class User
    {
        public User()
        {
            Id = -1;
            Login = string.Empty;
            PasswordHash = null;
            Level = 0;
        }

        public User(long id)
        {
            Id = id;
            Login = string.Empty;
            PasswordHash = null;
            Level = 0;
        }

        [Key]
        public long Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public int Level { get; set; }
    }
}
