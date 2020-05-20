using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using App.Main.DbEdit.Interfaces;
using System.Collections.ObjectModel;
using AuthorizationDB;
using SDiC.Authorization.Other;
using System.Collections;
using SDiC;

namespace App.Main.DbEdit
{
    public partial class UsersDbEditView : Window, IDbEditView
    {
        public UsersDbEditView()
        {
            InitializeComponent();
        }

        public event EventHandler<AddingNewItemEventArgs> AddItemAttempt;
        public event EventHandler<TableSelectedEventArgs> TableSelected;

        private dynamic source;
        dynamic IDbEditView.Source { set => this.Source = value; }
        public dynamic Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                MainDG.ItemsSource = null;
                MainDG.ItemsSource = source;
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

        private void MainDG_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            AddItemAttempt.Invoke(sender, e);
        }

        private void MainDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                e.Column.IsReadOnly = true;
            }
        }

        private void MainDG_GotFocus(object sender, RoutedEventArgs e)
        {
            TableSelected.Invoke(this, new TableSelectedEventArgs((sender as DataGrid).ItemsSource.GetType().GetGenericArguments()[0]));
        }

        private void GenerateHashBt_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Move to model
            var hash = string.IsNullOrWhiteSpace(HasherPB.Password) ? string.Empty : Hasher.GetHash(HasherPB.Password);
            TextCopy.Clipboard.SetText(hash);
            ConfirmPU.IsOpen = true;
        }
    }
}
