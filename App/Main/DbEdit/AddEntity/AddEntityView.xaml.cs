using App.Common.CustomEventArgs;
using App.Main.DbEdit.AddEntity.Interfaces;
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

namespace App.Main.DbEdit
{
    public partial class AddEntityView : Window, IAddEntityView
    {
        public AddEntityView()
        {
            InitializeComponent();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            // TODO: fix type handling, move data wrapping to controller
            string login = LoginTB.Text.ToLower().Trim();
            string passwordHash = Hasher.GetHash(PasswordTB.Password.Trim());
            string type = TypeCB.SelectedIndex == 0 ? "admin" : "user";
            var user = new Database.User() { Login = login, PasswordHash = passwordHash, Type = type };
            this.AddUserAttempt.Invoke(this, new NewUserEventArgs(user));
        }

        public event EventHandler<NewUserEventArgs> AddUserAttempt;

        public void ShowErrorMessageBox(string error)
        {
            MessageBox.Show(error, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
