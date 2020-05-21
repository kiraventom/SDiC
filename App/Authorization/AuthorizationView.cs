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
            window.LoginBt.Click += LoginBt_Click;
        }

        protected override Window Window => window as Window;
        private readonly AuthorizationWindow window = new AuthorizationWindow();

        public event EventHandler<LoginEventArgs> LoginAttempt;
        public event EventHandler<LoginEventArgs> SuccessfulLogin;

        private void LoginBt_Click(object sender, EventArgs e)
        {
            string login = window.LoginTB.Text.ToLower().Trim();
            string password = window.PasswordTB.Password.Trim();
            LoginAttempt.Invoke(this, new LoginEventArgs(new Credentials(login, password)));
        }

        public void ReactToLoginAttempt(bool loginSuccessful)
        {
            if (loginSuccessful)
            {
                string login = window.LoginTB.Text;
                string password = window.PasswordTB.Password;
                window.LoginTB.Clear();
                window.PasswordTB.Clear();
                SuccessfulLogin.Invoke(this, new LoginEventArgs(new Credentials(login, password)));
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
    }
}
