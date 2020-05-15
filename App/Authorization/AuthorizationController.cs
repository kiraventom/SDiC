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
            (View as Window).Closed += this.AuthorizationView_Closed;
        }

        IView IController.View => View as IView;
        IModel IController.Model => Model as IModel;

        private readonly IAuthorizationView View;
        private readonly IAuthorizationModel Model;

        public event EventHandler<ControllerClosedEventArgs> ControllerClosed;

        public void View_LoginAttempt(object sender, LoginEventArgs e)
        {
            bool isLoginSuccessful = Model.Login(e.Credentials);
            View.ReactToLoginAttempt(isLoginSuccessful);
        }

        public void View_SuccessfulLogin(object sender, LoginEventArgs e)
        {
            ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Success, Model.AuthorizedUser));
        }

        private void AuthorizationView_Closed(object sender, EventArgs e)
        {
            ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Abort));
        }

        public void Show() => View.Show();
        public void Close() => View.Hide();
    }
}
