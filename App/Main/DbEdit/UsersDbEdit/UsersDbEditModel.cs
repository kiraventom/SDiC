using App.Main.DbEdit.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace App.Main.DbEdit
{
    public class UsersDbEditModel : IDbEditModel
    {
        public UsersDbEditModel()
        {
            Context = new AuthorizationDB.UsersContext();
        }

        private readonly AuthorizationDB.UsersContext Context;

        dynamic IDbEditModel.ReadAll()
        {
            Context.Users.Load();
            return Context.Users.Local.ToObservableCollection();
        }

        void IDbEditModel.Save() => Context.SaveChanges();

        ~UsersDbEditModel() => Context.Dispose();
    }
}
