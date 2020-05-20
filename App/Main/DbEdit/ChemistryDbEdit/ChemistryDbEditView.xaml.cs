using App.Main.DbEdit.Interfaces;
using AuthorizationDB;
using SDiC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace App.Main.DbEdit.ChemistryDbEdit
{
    public partial class ChemistryDbEditView : Window, IDbEditView
    {
        //TODO: Move everything to ChemistryView class, which will be derived from base DbEditVIew class (same for Users View)
        public ChemistryDbEditView()
        {
            InitializeComponent();
        }

        public event EventHandler<AddingNewItemEventArgs> AddItemAttempt;
        public event EventHandler<TableSelectedEventArgs> TableSelected;

        private dynamic source;
        dynamic IDbEditView.Source
        {
            set
            {
                // I want to spit on the face of the one who wrote this code, but it's hard to spit on your own face
                MaterialsSource = value[0];
                ParametersSource = value[1];
                ParametersTypesSource = value[2];
                ParametersValuesSource = value[3];
                UnitsSource = value[4];
            }
        }

        public ObservableCollection<ChemistryDB.Materials> MaterialsSource
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                MaterialsDG.ItemsSource = null;
                MaterialsDG.ItemsSource = source;
            }
        }

        public ObservableCollection<ChemistryDB.Parameters> ParametersSource
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                ParametersDG.ItemsSource = null;
                ParametersDG.ItemsSource = source;
            }
        }

        public ObservableCollection<ChemistryDB.ParametersTypes> ParametersTypesSource
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                ParametersTypesDG.ItemsSource = null;
                ParametersTypesDG.ItemsSource = source;
            }
        }

        public ObservableCollection<ChemistryDB.ParametersValues> ParametersValuesSource
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                ParametersValuesDG.ItemsSource = null;
                ParametersValuesDG.ItemsSource = source;
            }
        }

        public ObservableCollection<ChemistryDB.Units> UnitsSource
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                UnitsDG.ItemsSource = null;
                UnitsDG.ItemsSource = source;
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
