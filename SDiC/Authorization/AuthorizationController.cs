using SDiC.Authorization.Interfaces;
using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        }

        IView IController.View => View as IView;
        IModel IController.Model => Model as IModel;

        public IAuthorizationView View { get; }
        public IAuthorizationModel Model { get; }
        public event EventHandler<Common.FormClosingEventArgs> FormClosing;

        public void View_LoginAttempt(object sender, LoginEventArgs e)
        {
            bool isLoginSuccessful = Model.Login(e.Credentials);
            View.LoginAttemptResult(isLoginSuccessful);
        }

        public void View_SuccessfulLogin(object sender, LoginEventArgs e)
        {
            FormClosing.Invoke(this, new Common.FormClosingEventArgs(Common.FormClosingEventArgs.CloseReason.Success, e.Credentials));
        }

        public void Show() => View.Show();
        public void Close() => View.Hide();
    }
}
