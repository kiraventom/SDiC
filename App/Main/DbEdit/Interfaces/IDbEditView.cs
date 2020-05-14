using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Main.DbEdit.Interfaces
{
    public interface IDbEditView : IView
    {
        IEnumerable<Database.User> Source { set; }

        event EventHandler AddUserRequest;
        event EventHandler UpdateDbRequest;

        bool? ConfirmChanges();
    }
}
