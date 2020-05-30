using App.DbEdit.Abstraction;
using System.Windows;

namespace App.DbEdit.Chemistry
{
    public class ChemistryDbEditView : DbEditView
    {
        public ChemistryDbEditView() : base()
        {
        }

        protected override Window Window => chemistryWindow;
        private readonly ChemistryDbEditWindow chemistryWindow = new ChemistryDbEditWindow();
    }
}
