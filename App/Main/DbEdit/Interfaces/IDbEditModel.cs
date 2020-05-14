using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace App.Main.DbEdit.Interfaces
{
    public interface IDbEditModel : IModel
    {
        List<Database.User> ReadAll();
        void Add(Database.User newUser);
        void Remove(Database.User removedUser);
        void Save();
    }
}
