using System;
using System.Collections.Generic;
using System.Text;

namespace App.ResultsTable
{
    public class ResultsTableModel : Common.Abstraction.Model
    {
        public ResultsTableModel(IEnumerable<double> z, IEnumerable<double> T, IEnumerable<double> eta)
        {
            this.z = z;
            this.T = T;
            this.eta = eta;
        }

        public IEnumerable<double> z { get; }
        public IEnumerable<double> T { get; }
        public IEnumerable<double> eta { get; }
    }
}
