using App.Common.Abstraction;
using App.Common.CustomEventArgs;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OxyPlot;

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

            var duds = mainWindow.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => field.FieldType == typeof(DoubleUpDown))
                .Select(field => field.GetValue(mainWindow))
                .Cast<DoubleUpDown>();
            foreach (var dud in duds)
            {
                dud.ValueChanged += InputParamsDUD_ValueChanged;
            }

            mainWindow.SolveBt.Click += this.SolveBt_Click;
        }

        protected override Window Window => mainWindow;
        private readonly MainWindow mainWindow = new MainWindow();

        public IEnumerable Materials { set => mainWindow.MaterialCB.ItemsSource = value; }

        public event EventHandler SignOut;
        public event EventHandler EditUsersDb;
        public event EventHandler EditChemistryDb;
        public event EventHandler WindowLoaded;
        public event EventHandler<CustomEventArgs> MaterialChanged;
        public event EventHandler<ParameterChangedEventArgs> ParameterChanged;
        public event EventHandler SolveBtClicked;

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

        private void SolveBt_Click(object sender, RoutedEventArgs e)
        {
            SolveBtClicked.Invoke(this, EventArgs.Empty);
        }

        public void SetOutputValues(MathModel.Solution solution)
        {
            mainWindow.F_TB.Text = solution.F.ToString();
            mainWindow.Q_CH_TB.Text = solution.Q_CH.ToString();
            mainWindow.Gamma_TB.Text = solution.gamma.ToString();
            mainWindow.Q_Gamma_TB.Text = solution.q_gamma.ToString();
            mainWindow.Q_Alpha_TB.Text = solution.q_alpha.ToString();
            mainWindow.N_TB.Text = solution.N.ToString();
            mainWindow.Q_TB.Text = solution.Q.ToString();
            mainWindow.T_p_TB.Text = solution.T_p.ToString();
            mainWindow.Eta_p_TB.Text = solution.eta_p.ToString();
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
        }

        private static void ShowErrorMessage()
        {
            System.Windows.MessageBox.Show($"Все поля ввода должны быть заполнены",
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
