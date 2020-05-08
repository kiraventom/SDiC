using SDiC.Authorization.Interfaces;
using SDiC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDiC
{
    public partial class AuthorizationView : Form, IAuthorizationView
    {
        public AuthorizationView()
        {
            InitializeComponent();
        }

        public event EventHandler<LoginEventArgs> LoginAttempt;
        public event EventHandler<LoginEventArgs> SuccessfulLogin;

        private void LoginBt_Click(object sender, EventArgs e)
        {
            LoginAttempt.Invoke(this, new LoginEventArgs(new Credentials(LoginTB.Text, PasswordTB.Text)));
        }

        public void LoginAttemptResult(bool credentialsOK)
        {
            if (credentialsOK)
            {
                SuccessfulLogin.Invoke(this, new LoginEventArgs(new Credentials(LoginTB.Text, PasswordTB.Text)));
                LoginTB.Clear();
                PasswordTB.Clear();
            }
            else
            {
                MessageBox.Show("Ошибка!");
            }
        }
    }
}
