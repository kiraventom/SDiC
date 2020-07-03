using App.Common.Abstraction;
using App.Common.CustomEventArgs;
using OxyPlot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace App.Main
{
    public sealed class MainView : View
    {
        public MainView() : base()
        {
            mainWindow.SignOutBt.Click += SignOutBt_Click;
            mainWindow.EditUsersDbBt.Click += EditUsersDbBt_Click;
            mainWindow.EditChemistryDbBt.Click += EditChemistryDbBt_Click;
            mainWindow.Loaded += this.Window_Loaded;

            mainWindow.MaterialCB.SelectionChanged += this.MaterialCB_SelectionChanged;

            DUDs = new ReadOnlyCollection<DoubleUpDown>( 
                   mainWindow.GetType()
                   .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                   .Where(field => field.FieldType == typeof(DoubleUpDown))
                   .Select(field => field.GetValue(mainWindow))
                   .Cast<DoubleUpDown>()
                   .ToList());

            foreach (var dud in DUDs)
            {
                dud.ValueChanged += InputParamsDUD_ValueChanged;
            }

            mainWindow.SolveBt.Click += this.SolveBt_Click;
            mainWindow.SaveReportBt.Click += this.SaveReportBt_Click;
            mainWindow.ResultsTableBt.Click += this.ResultsTableBt_Click;

            mainWindow.Loaded += this.MainWindow_Loaded;
        }

        protected override Window Window => mainWindow;
        private readonly MainWindow mainWindow = new MainWindow();
        private ReadOnlyCollection<DoubleUpDown> DUDs { get; }

        public IEnumerable Materials { set => mainWindow.MaterialCB.ItemsSource = value; }

        public event EventHandler SignOut;
        public event EventHandler EditUsersDb;
        public event EventHandler EditChemistryDb;
        public event EventHandler WindowLoaded;
        public event EventHandler<CustomEventArgs> MaterialChanged;
        public event EventHandler<ParameterChangedEventArgs> ParameterChanged;
        public event EventHandler SolveRequest;
        public event EventHandler SaveReportRequest;
        public event EventHandler ShowResultTableRequest;

        private void Window_Loaded(object sender, RoutedEventArgs e) => WindowLoaded.Invoke(this, e);

        private void MaterialCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMaterialName = (sender as ComboBox).SelectedItem.ToString();
            MaterialChanged.Invoke(this, new CustomEventArgs(selectedMaterialName));
        }

        private void InputParamsDUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var dud = sender as DoubleUpDown;
            var selectedParameterName = dud.Tag.ToString();
            var selectedParameterType = (dud.Parent as FrameworkElement).Tag.ToString();
            var value = dud.Value;
            if (value.HasValue)
            {
                ParameterChanged.Invoke(this, new ParameterChangedEventArgs(selectedParameterType, selectedParameterName, value.Value));
            }
        }

        private void ResultsTableBt_Click(object sender, RoutedEventArgs e)
        {
            ShowResultTableRequest.Invoke(this, EventArgs.Empty);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // TEMP 
            // TODO: REMOVE
            {
                mainWindow.Width_DUD.Value = 0.39;
                mainWindow.Height_DUD.Value = 0.01;
                mainWindow.Length_DUD.Value = 10.5;
                mainWindow.LidTemperature_DUD.Value = 180;
                mainWindow.LidSpeed_DUD.Value = 2.1;
                mainWindow.CalculationStep_DUD.Value = 0.005;
            }
        }

        // TEMP
        // TODO: FIX
        public void SetCalculationTime(long ms) => mainWindow.CalculationTime_TB.Text = ms.ToString();

        private void SolveBt_Click(object sender, RoutedEventArgs e)
        {
            if (DUDs.Any(dud => dud.Value is null))
            {
                ShowInputErrorMessage();
                return;
            }
            else
            {
                SolveRequest.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetOutputValues(MathModel.Solution solution)
        {
            mainWindow.F_TB.Text = solution.ChannelGeometricShapeCoefficient.ToString();
            mainWindow.Q_CH_TB.Text = solution.ForwardFlowMaterialConsumption.ToString();
            mainWindow.Gamma_TB.Text = solution.MaterialShearStrainRate.ToString();
            mainWindow.Q_Gamma_TB.Text = solution.SpecificHeatFluxForStreamViscousFriction.ToString();
            mainWindow.Q_Alpha_TB.Text = solution.SpecificHeatFluxForChannelLidHeatExchange.ToString();
            mainWindow.N_TB.Text = solution.CalculationStepAmount.ToString();
            mainWindow.Q_TB.Text = solution.ChannelProductivity.ToString();
            mainWindow.T_p_TB.Text = solution.ChannelTemperature.ToString();
            mainWindow.Eta_p_TB.Text = solution.ProductViscosity.ToString();
        }

        public void SetOutputCharts(IEnumerable<DataPoint> etaValues, IEnumerable<DataPoint> TValues)
        {
            foreach (var axis in mainWindow.Eta_Plt.Axes)
            {
                switch (axis.Position)
                {
                    case OxyPlot.Axes.AxisPosition.Bottom:
                    case OxyPlot.Axes.AxisPosition.Top:
                        axis.Minimum = etaValues.Min(dp => dp.X);
                        axis.Maximum = etaValues.Max(dp => dp.X);
                        break;
                    case OxyPlot.Axes.AxisPosition.Left:
                    case OxyPlot.Axes.AxisPosition.Right:
                        axis.Minimum = etaValues.Min(dp => dp.Y);
                        axis.Maximum = etaValues.Max(dp => dp.Y);
                        break;
                }
            }
            foreach (var axis in mainWindow.T_Plt.Axes)
            {
                switch (axis.Position)
                {
                    case OxyPlot.Axes.AxisPosition.Bottom:
                    case OxyPlot.Axes.AxisPosition.Top:
                        axis.Minimum = TValues.Min(dp => dp.X);
                        axis.Maximum = TValues.Max(dp => dp.X);
                        break;
                    case OxyPlot.Axes.AxisPosition.Left:
                    case OxyPlot.Axes.AxisPosition.Right:
                        axis.Minimum = TValues.Min(dp => dp.Y);
                        axis.Maximum = TValues.Max(dp => dp.Y);
                        break;
                }
            }
            mainWindow.Eta_Plt.Series[0].ItemsSource = etaValues;
            mainWindow.T_Plt.Series[0].ItemsSource = TValues;
            mainWindow.Eta_Plt.InvalidatePlot(true);
            mainWindow.T_Plt.InvalidatePlot(true);
        }

        private void SaveReportBt_Click(object sender, RoutedEventArgs e)
        {
            SaveReportRequest.Invoke(this, EventArgs.Empty);
        }

        private static void ShowInputErrorMessage()
        {
            System.Windows.MessageBox.Show($"Все поля ввода должны быть заполнены",
                                           "Ошибка!",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Error);
        }

        public static void ShowOutputErrorMessage()
        {
            System.Windows.MessageBox.Show($"Сначала произведите вычисление",
                                           "Ошибка!",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Error);
        }

        private void SignOutBt_Click(object sender, RoutedEventArgs e) => SignOut.Invoke(this, EventArgs.Empty);

        private void EditUsersDbBt_Click(object sender, RoutedEventArgs e) => EditUsersDb.Invoke(this, EventArgs.Empty);

        private void EditChemistryDbBt_Click(object sender, RoutedEventArgs e) => EditChemistryDb.Invoke(this, EventArgs.Empty);

        public static bool ConfirmSigningOut()
        {
            var mbr = System.Windows.MessageBox.Show("Вы действительно хотите выйти из аккаунта?",
                                                     "Подтверждение",
                                                     MessageBoxButton.YesNo,
                                                     MessageBoxImage.Question);
            return mbr == MessageBoxResult.Yes;
        }

        const string greetingStart = "Здравствуйте, ";
        public string Greeting { set => mainWindow.GreetingsL.Content = greetingStart + value; }

        public bool IsAdmin
        {
            set
            {
                mainWindow.EditUsersDbBt.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                mainWindow.EditChemistryDbBt.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                mainWindow.ScientistPanelGB.Visibility = value ? Visibility.Hidden : Visibility.Visible;
            }
        }
    }
}
