using App.DbEdit.Abstraction;

namespace App.DbEdit.Chemistry
{
    public class ChemistryDbEditController : DbEditController
    {
        public ChemistryDbEditController(DbEditView view, DbEditModel model) : base(view, model)
        {
            CurrentType = typeof(ChemistryDB.Materials);
        }
    }
}
