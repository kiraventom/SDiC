using App.Common.Abstraction;
using System;
using System.Windows;

namespace App.Main
{
    public sealed class MainView : View
    {
        public MainView()
        {
            window.SignOutBt.Click += SignOutBt_Click;
            window.EditUsersDbBt.Click += EditUsersDbBt_Click;
            window.EditChemistryDbBt.Click += EditChemistryDbBt_Click;
        }

        public event EventHandler SignOut;
        public event EventHandler EditUsersDb;
        public event EventHandler EditChemistryDb;

        protected override Window Window => window as Window;
        private readonly MainWindow window = new MainWindow();

        private void SignOutBt_Click(object sender, RoutedEventArgs e)
        {
            SignOut.Invoke(this, EventArgs.Empty);
        }

        private void EditUsersDbBt_Click(object sender, RoutedEventArgs e)
        {
            EditUsersDb.Invoke(this, EventArgs.Empty);
        }

        private void EditChemistryDbBt_Click(object sender, RoutedEventArgs e)
        {
            EditChemistryDb.Invoke(this, EventArgs.Empty);
        }

        public static bool ConfirmSigningOut()
        {
            var mbr = MessageBox.Show("Вы действительно хотите выйти из аккаунта?",
                                      "Подтверждение",
                                      MessageBoxButton.YesNo,
                                      MessageBoxImage.Question);
            return mbr == MessageBoxResult.Yes;
        }

        const string greetingStart = "Здравствуйте, ";
        public string Greeting
        {
            set
            {
                window.GreetingsL.Content = greetingStart + value;
            }
        }

        public bool IsEditDbBtsVisible
        {
            set
            {
                window.EditUsersDbBt.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                window.EditChemistryDbBt.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}
