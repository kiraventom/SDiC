using SDiC.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Application
{
    public partial class MainView : Window, IMainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void SignOutBt_Click(object sender, RoutedEventArgs e)
        {
            SignOut.Invoke(this, EventArgs.Empty);
        }

        private void EditDbBt_Click(object sender, RoutedEventArgs e)
        {
            EditDb.Invoke(this, EventArgs.Empty);
        }

        public bool ConfirmSigningOut()
        {
            var mbr = MessageBox.Show("Вы действительно хотите выйти из аккаунта?",
                                      "Подтверждение",
                                      MessageBoxButton.YesNo,
                                      MessageBoxImage.Question);
            return mbr == MessageBoxResult.Yes;
        }

        public event EventHandler SignOut;
        public event EventHandler EditDb;

        const string greeting = "Здравствуйте, ";
        public string Name
        {
            set
            {
                GreetingsL.Content = greeting + value;
            }
        }

        public bool IsEditDbBtVisible
        {
            set
            {
                EditDbBt.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}
