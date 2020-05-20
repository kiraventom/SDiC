using SDiC;
using SDiC.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace App.Main.DbEdit.Interfaces
{
    public interface IDbEditView : IView
    {
        dynamic Source { set; }
        event EventHandler<AddingNewItemEventArgs> AddItemAttempt;
        event EventHandler<TableSelectedEventArgs> TableSelected;

        bool? ConfirmChanges();
    }
}
