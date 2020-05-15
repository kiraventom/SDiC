using App.Main.DbEdit.AddEntity.Interfaces;
using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace App.Main.DbEdit.AddEntity
{
    public class AddEntityController : IAddEntityController
    {
        public AddEntityController(IAddEntityView view, IAddEntityModel model)
        {
            View = view;
            Model = model;
            (View as Window).Closing += this.AddEntityView_Closing;
            View.AddUserAttempt += this.View_AddUserAttempt;
        }

        IView IController.View => View as IView;
        IModel IController.Model => Model as IModel;

        private readonly IAddEntityView View;
        private readonly IAddEntityModel Model;

        public event EventHandler<ControllerClosedEventArgs> ControllerClosed;

        private void View_AddUserAttempt(object sender, Common.CustomEventArgs.NewUserEventArgs e)
        {
            switch (Model.CheckNewUser(e.NewUser))
            {
                case AddEntityModel.Error.EmptyLogin:
                    View.ShowErrorMessageBox("Логин не может быть пустым или состоять из пробелов");
                    break;

                case AddEntityModel.Error.ExistingLogin:
                    View.ShowErrorMessageBox("Аккаунт с таким логином уже существует"); 
                    break;

                case AddEntityModel.Error.EmptyPassword:
                    View.ShowErrorMessageBox("Пароль не может быть пустым или состоять из пробелов");
                    break;

                case AddEntityModel.Error.EmptyType:
                    View.ShowErrorMessageBox("Тип не может быть пустым или состоять из пробелов");
                    break;

                case AddEntityModel.Error.None:
                    ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Success, e.NewUser));
                    break;
            }
        }

        private void AddEntityView_Closing(object sender, EventArgs e)
        {
            ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Abort));
        }

        public void Show() => View.ShowDialog();
        public void Close() => View.Close();
    }
}
