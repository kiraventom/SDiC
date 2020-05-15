using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace App.Main.DbEdit.Interfaces
{
    public interface IDbEditModel : IModel
    {
        ObservableCollection<Database.User> ReadAll();
        void Save();
    }
}
