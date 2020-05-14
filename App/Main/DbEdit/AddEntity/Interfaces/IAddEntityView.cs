using App.Common.CustomEventArgs;
using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Main.DbEdit.AddEntity.Interfaces
{
    public interface IAddEntityView : IView
    {
        event EventHandler<NewUserEventArgs> AddUserAttempt;

        void ShowErrorMessageBox(string error);
    }
}
