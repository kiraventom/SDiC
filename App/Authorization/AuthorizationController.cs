using App.Authorization;
using App.Common;
using App.Common.Abstraction;
using App.Common.CustomEventArgs;
using System;
using System.Windows;

namespace App.Authorization
{
    public class AuthorizationController : Controller
    {
        public AuthorizationController(AuthorizationView view, AuthorizationModel model)
        {
            this.view = view;
            this.model = model;

            this.view.LoginAttempt += this.View_LoginAttempt;
            this.view.SuccessfulLogin += this.View_SuccessfulLogin;
            this.view.Closing += AuthorizationView_Closing;
        }

        protected override View View => view as View;
        protected override Model Model => model as Model;

        private readonly AuthorizationView view;
        private readonly AuthorizationModel model;

        public override event EventHandler<ControllerClosedEventArgs> ControllerClosed;

        public void View_LoginAttempt(object sender, LoginEventArgs e)
        {
            bool isLoginSuccessful = model.Login(e.Credentials);
            view.ReactToLoginAttempt(isLoginSuccessful);
        }

        public void View_SuccessfulLogin(object sender, LoginEventArgs e)
        {
            ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Success, model.AuthorizedUser));
        }

        private void AuthorizationView_Closing(object sender, EventArgs e)
        {
            ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Abort));
        }

        public override void Show() => view.Show();
        public override void Close() => view.Hide();
    }
}
