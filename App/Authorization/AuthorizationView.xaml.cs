using SDiC;
using SDiC.Authorization.Interfaces;
using SDiC.Authorization.Other;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Application.Authorization
{
    /// <summary>
    /// Interaction logic for AuthorizationView.xaml
    /// </summary>
    public partial class AuthorizationView : Window, IAuthorizationView
    {
        public AuthorizationView()
        {
            InitializeComponent();
        }

        public event EventHandler<LoginEventArgs> LoginAttempt;
        public event EventHandler<LoginEventArgs> SuccessfulLogin;

        private void LoginBt_Click(object sender, EventArgs e)
        {
            string login = LoginTB.Text;
            string passwordHash = Hasher.GetHash(PasswordTB.Password);
            LoginAttempt.Invoke(this, new LoginEventArgs(new Credentials(login, passwordHash)));
        }

        public void LoginAttemptResult(bool credentialsOK)
        {
            if (credentialsOK)
            {
                string login = LoginTB.Text;
                string passwordHash = Hasher.GetHash(PasswordTB.Password);
                LoginTB.Clear();
                PasswordTB.Clear();
                SuccessfulLogin.Invoke(this, new LoginEventArgs(new Credentials(login, passwordHash)));
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
    }
}
