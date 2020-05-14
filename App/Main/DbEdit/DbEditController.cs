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
            Users = new ObservableCollection<Database.User>(Model.ReadAll());
            Users.CollectionChanged += this.Users_CollectionChanged;
            (View as Window).Loaded += this.DbEditView_Loaded;
            (View as Window).Closing += this.DbEditView_Closing;
            View.AddUserRequest += this.View_AddUserRequest;
            View.UpdateDbRequest += this.View_UpdateDbRequest;
        }

        private void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) 
        { 
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    Model.Remove(e.OldItems[0] as Database.User);
                    break;
                default:
                    return;
            }
        }

        readonly ObservableCollection<Database.User> Users;

        private void View_AddUserRequest(object sender, EventArgs e)
        {
            var logins =
                from user in Model.ReadAll()
                select user.Login;
            IAddEntityController addEntityController 
                = new AddEntityController(new AddEntityView(), new AddEntityModel(), logins);
            addEntityController.ControllerClosed += (sender, ea) =>
            {
                if (ea.Reason == ControllerClosedEventArgs.CloseReason.Success
                    && ea.Data != null)
                {
                    var user = ea.Data as Database.User;
                    Users.Add(user);
                    Model.Add(user);
                    addEntityController.Close();
                }
            };
            addEntityController.Show();
        }

        private void View_UpdateDbRequest(object sender, EventArgs e)
        {
            Model.Save();
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
