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
using App.Main.DbEdit.Interfaces;

namespace App.Main.DbEdit
{
    /// <summary>
    /// Interaction logic for DbEditView.xaml
    /// </summary>
    public partial class DbEditView : Window, IDbEditView
    {
        public DbEditView()
        {
            InitializeComponent();
            Source = new List<Database.User>();
        }

        public IEnumerable<Database.User> Source
        {
            set
            {
                MainDG.ItemsSource = value;
                foreach (var column in MainDG.Columns)
                    column.IsReadOnly = true;
            }
        }

        public bool? ConfirmChanges()
        {
            var mbr = MessageBox.Show("Сохранить изменения?",
                                      "Подтверждение",
                                      MessageBoxButton.YesNoCancel,
                                      MessageBoxImage.Question);
            return (mbr) switch
            {
                MessageBoxResult.Yes => true,
                MessageBoxResult.No => false,
                MessageBoxResult.Cancel => null,
                _ => throw new NotImplementedException($"MessageBoxResult \"{mbr.ToString()}\" is not \"Yes\", \"No\" or \"Cancel\"")
            };
        }

        private void AddUserBt_Click(object sender, RoutedEventArgs e)
        {
            AddUserRequest.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler AddUserRequest;
        public event EventHandler UpdateDbRequest;

        private void UpdateBt_Click(object sender, RoutedEventArgs e)
        {
            UpdateDbRequest.Invoke(this, EventArgs.Empty);
        }
    }
}
