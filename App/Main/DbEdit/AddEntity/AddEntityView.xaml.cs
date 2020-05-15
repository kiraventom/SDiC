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
            string login = LoginTB.Text.ToLower().Trim();
            string password = PasswordTB.Password.Trim();
            int level = TypeCB.SelectedIndex;
            this.AddUserAttempt.Invoke(this, new NewUserEventArgs(login, password, level));
        }

        public event EventHandler<NewUserEventArgs> AddUserAttempt;

        public void ShowErrorMessageBox(string error)
        {
            MessageBox.Show(error, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
