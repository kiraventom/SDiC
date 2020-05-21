using App.Common.Abstraction;
using App.Common.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace App.DbEdit.Abstraction
{
    public abstract class DbEditView : View
    {
        protected DbEditView() : base()
        {
            foreach (var dataGrid in DataGrids)
            {
                dataGrid.AddingNewItem += DataGrid_AddingNewItem;
                dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
                dataGrid.GotFocus += DataGrid_GotFocus;
            }
        }

        private IEnumerable<DataGrid> DataGrids => 
            (Window.Content as Panel).Children
            .Cast<FrameworkElement>()
            .OfType<DataGrid>();

        public event EventHandler<AddingNewItemEventArgs> AddItemAttempt;
        public event EventHandler<TableSelectedEventArgs> TableSelected;

        public dynamic Source
        {
            set
            {
                var tables = value; // List<ObservableCollection<EntityType>>
                foreach (var table in tables)
                {
                    Type tableType = table.GetType(); // ObservableCollection<EntityType>
                    Type entityType = tableType.GetGenericArguments()[0]; // EntityType
                    var dataGrid = DataGrids
                        .First(dg => dg.Name
                        .Contains(entityType.Name, StringComparison.OrdinalIgnoreCase));
                    dataGrid.ItemsSource = null; // otherwise DataGrid won't update
                    dataGrid.ItemsSource = table;
                }
            }
        }

        public static bool? ConfirmChanges()
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

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            AddItemAttempt.Invoke(sender, e);
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                e.Column.IsReadOnly = true;
            }
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            TableSelected.Invoke(this, new TableSelectedEventArgs((sender as DataGrid).ItemsSource.GetType().GetGenericArguments()[0]));
        }
    }
}
