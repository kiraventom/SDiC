using App.Authorization.Other;
using App.Common.Abstraction;
using App.Common.CustomEventArgs;
using System;
using System.Windows;

namespace App.Authorization
{
    public class AuthorizationView : View
    {
        public AuthorizationView() : base()
        {
            authorizationWindow.LoginBt.Click += LoginBt_Click;
        }

        protected override Window Window => authorizationWindow;
        private readonly AuthorizationWindow authorizationWindow = new AuthorizationWindow();

        public event EventHandler<LoginEventArgs> LoginAttempt;
        public event EventHandler<LoginEventArgs> SuccessfulLogin;

        private void LoginBt_Click(object sender, EventArgs e)
        {
            string login = authorizationWindow.LoginTB.Text.ToLower().Trim();
            string password = authorizationWindow.PasswordTB.Password.Trim();
            LoginAttempt.Invoke(this, new LoginEventArgs(new Credentials(login, password)));
        }

        public void ReactToLoginAttempt(bool loginSuccessful)
        {
            if (loginSuccessful)
            {
                string login = authorizationWindow.LoginTB.Text;
                string password = authorizationWindow.PasswordTB.Password;
                authorizationWindow.LoginTB.Clear();
                authorizationWindow.PasswordTB.Clear();
                SuccessfulLogin.Invoke(this, new LoginEventArgs(new Credentials(login, password)));
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
    }
}
