using SDiC.Authorization.Interfaces;
using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SDiC
{
    class AuthorizationController : IAuthorizationController
    {
        public AuthorizationController(IAuthorizationView view, IAuthorizationModel model)
        {
            View = view;
            Model = model;

            View.LoginAttempt += this.View_LoginAttempt;
            View.SuccessfulLogin += this.View_SuccessfulLogin;
            (View as Window).Closed += this.AuthorizationController_Closed;
        }

        IView IController.View => View as IView;
        IModel IController.Model => Model as IModel;

        public IAuthorizationView View { get; }
        public IAuthorizationModel Model { get; }
        public event EventHandler<WindowClosingEventArgs> WindowClosed;
        private Database.User AuthorizedUser = null;

        public void View_LoginAttempt(object sender, LoginEventArgs e)
        {
            AuthorizedUser = Model.Login(e.Credentials);
            bool isLoginSuccessful = AuthorizedUser != null;
            View.LoginAttemptResult(isLoginSuccessful);
        }

        public void View_SuccessfulLogin(object sender, LoginEventArgs e)
        {
            WindowClosed.Invoke(this, new WindowClosingEventArgs(WindowClosingEventArgs.CloseReason.Success, AuthorizedUser));
        }

        private void AuthorizationController_Closed(object sender, EventArgs e)
        {
            WindowClosed.Invoke(this, new WindowClosingEventArgs(WindowClosingEventArgs.CloseReason.Abort));
        }

        public void Show() => View.Show();
        public void Close() => View.Hide();
    }
}
