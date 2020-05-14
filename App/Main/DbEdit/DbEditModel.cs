using App.Main.DbEdit.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace App.Main.DbEdit
{
    public class DbEditModel : IDbEditModel
    {
        public DbEditModel()
        {
            Context = new Database.UsersContext();
        }

        private readonly Database.UsersContext Context;

        public List<Database.User> ReadAll()
        {
            var query =
                from user in Context.Users
                orderby user.Type
                select user;
            return query.ToList();
        }

        public void Add(Database.User newUser) => Context.Users.Add(newUser);

        public void Remove(Database.User removedUser) => Context.Users.Remove(removedUser);

        public void Save() => Context.SaveChanges();

        //TODO: Check Context.Users.Local

        ~DbEditModel() => Context.Dispose();
    }
}
