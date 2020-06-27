using App.Common.Abstraction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace App.ResultsTable
{
    public class ResultsTableView : Common.Abstraction.View
    {
        public ResultsTableView()
        {
            window.Loaded += this.Window_Loaded;
        }

        protected override Window Window => window;
        private readonly ResultsTableWindow window = new ResultsTableWindow();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Loaded;

        public void FillTables(IEnumerable<double> z, IEnumerable<double> T, IEnumerable<double> eta)
        {
            double z_i;
            double T_i;
            double eta_i;
            for (int i = 0; i < z.Count(); ++i)
            {
                z_i = Math.Round(z.ElementAt(i), 3);
                T_i = Math.Round(T.ElementAt(i), 3);
                eta_i = Math.Round(eta.ElementAt(i), 3);
                window.ResultsDG.Items.Add( new { _z = z_i, _T = T_i, _eta = eta_i });
            }
        }
    }
}
