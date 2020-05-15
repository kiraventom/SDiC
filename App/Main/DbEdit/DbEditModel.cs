using App.Main.DbEdit.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Database.User> ReadAll()
        {
            Context.Users.Load();
            return Context.Users.Local.ToObservableCollection();
        }

        public void Save() => Context.SaveChanges();

        ~DbEditModel() => Context.Dispose();
    }
}
