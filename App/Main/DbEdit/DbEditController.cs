using App.Main.DbEdit.Interfaces;
using SDiC.Common;
using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using App.Main.DbEdit.AddEntity.Interfaces;
using App.Main.DbEdit.AddEntity;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace App.Main.DbEdit
{
    public class DbEditController : IDbEditController
    {
        public DbEditController(IDbEditView view, IDbEditModel model)
        {
            View = view;
            Model = model;
            Users = Model.ReadAll();
            (View as Window).Loaded += this.DbEditView_Loaded;
            (View as Window).Closing += this.DbEditView_Closing;
            View.AddUserRequest += this.View_AddUserRequest;
        }

        readonly ObservableCollection<Database.User> Users;

        private void View_AddUserRequest(object sender, EventArgs e)
        {
            var logins = Users.Select(u => u.Login);
            IAddEntityController addEntityController 
                = new AddEntityController(new AddEntityView(), new AddEntityModel(logins));
            addEntityController.ControllerClosed += (sender, ea) =>
            {
                if (ea.Reason == ControllerClosedEventArgs.CloseReason.Success
                    && ea.Data != null)
                {
                    var user = ea.Data as Database.User;
                    Users.Add(user);
                    addEntityController.Close();
                }
            };
            addEntityController.Show();
        }

        private void DbEditView_Loaded(object sender, RoutedEventArgs e)
        {
            View.Source = Users;
        }

        private void DbEditView_Closing(object sender, CancelEventArgs e)
        {
            ControllerClosedEventArgs.CloseReason closeReason;
            switch (View.ConfirmChanges())
            {
                case true:
                    closeReason = ControllerClosedEventArgs.CloseReason.Success;
                    Model.Save();
                    break;
                case false:
                    closeReason = ControllerClosedEventArgs.CloseReason.Abort;
                    break;
                case null:
                    e.Cancel = true;
                    return;
            }

            ControllerClosed.Invoke(this, new ControllerClosedEventArgs(closeReason));
        }

        IView IController.View => View as IView;
        IModel IController.Model => Model as IModel;
        private readonly IDbEditView View;
        private readonly IDbEditModel Model;

        public event EventHandler<ControllerClosedEventArgs> ControllerClosed;
        public void Show() => View.ShowDialog();
        public void Close() => View.Close();
    }
}
