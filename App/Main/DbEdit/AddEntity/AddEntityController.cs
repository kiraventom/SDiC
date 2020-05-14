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
        public AddEntityController(IAddEntityView view, IAddEntityModel model, IEnumerable<string> logins)
        {
            View = view;
            Model = model;
            Logins = logins.ToList();
            (View as Window).Closing += this.AddEntityView_Closing;
            View.AddUserAttempt += this.View_AddUserAttempt;
        }

        private readonly IReadOnlyCollection<string> Logins;

        IView IController.View => View as IView;
        IModel IController.Model => Model as IModel;

        private readonly IAddEntityView View;
        private readonly IAddEntityModel Model;

        public event EventHandler<ControllerClosedEventArgs> ControllerClosed;

        private void View_AddUserAttempt(object sender, Common.CustomEventArgs.NewUserEventArgs e)
        {
            // TODO: Move error handling to model
            if (string.IsNullOrWhiteSpace(e.NewUser.Login))
            {
                View.ShowErrorMessageBox("Логин не может быть пустым или состоять из пробелов");
            }
            else
            if (Logins.Any(login => login.Equals(e.NewUser.Login, StringComparison.OrdinalIgnoreCase)))
            {
                View.ShowErrorMessageBox("Аккаунт с таким логином уже существует");
            }
            else
            if (string.IsNullOrWhiteSpace(e.NewUser.PasswordHash))
            {
                View.ShowErrorMessageBox("Пароль не может быть пустым или состоять из пробелов");
            }
            else
            if (string.IsNullOrWhiteSpace(e.NewUser.Type))
            {
                View.ShowErrorMessageBox("Тип не может быть пустым или состоять из пробелов");
            }
            else // все данные в порядке, можно добавлять аккаунт
            {
                ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Success, e.NewUser));
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
