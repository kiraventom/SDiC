using App.DbEdit.Abstraction;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace App.DbEdit.Chemistry
{
    public class ChemistryDbEditView : DbEditView
    {
        public ChemistryDbEditView() : base()
        {
        }

        protected override Window Window => chemistryWindow;
        private readonly ChemistryDbEditWindow chemistryWindow = new ChemistryDbEditWindow();

        public override void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // TEMP
            // TODO: REMOVE
            e.Cancel = headers.Any(h => h == e.Column.Header.ToString());
        }

        private readonly string[] headers = new string[] { "ParameterValue", "Type", "Unit", "Parameter", "Material" };
    }
}
