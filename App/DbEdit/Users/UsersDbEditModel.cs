using App.DbEdit.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace App.DbEdit.Users
{
    public class UsersDbEditModel : DbEditModel
    {
        public UsersDbEditModel() : base()
        {
            Context = new AuthorizationDB.UsersContext();
        }

        protected override dynamic Context { get => context; set => context = value; }
        private AuthorizationDB.UsersContext context;

        public override dynamic GetAllTables()
        {
            List<dynamic> tables = new List<dynamic>();
            context.Users.Load();
            tables.Add(context.Users.Local.ToObservableCollection());
            return tables;
        }
    }
}
