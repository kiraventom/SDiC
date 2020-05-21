using App.DbEdit.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace App.DbEdit.Chemistry
{
    public class ChemistryDbEditModel : DbEditModel
    {
        public ChemistryDbEditModel() : base()
        {
            Context = new ChemistryDB.ChemistryContext();
        }

        protected override dynamic Context { get => context; set => context = value; }
        private ChemistryDB.ChemistryContext context;

        public override dynamic GetAllTables()
        {
            List<dynamic> tables = new List<dynamic>();
            context.Materials.Load();
            tables.Add(context.Materials.Local.ToObservableCollection());
            context.Parameters.Load();
            tables.Add(context.Parameters.Local.ToObservableCollection());
            context.ParametersTypes.Load();
            tables.Add(context.ParametersTypes.Local.ToObservableCollection());
            context.ParametersValues.Load();
            tables.Add(context.ParametersValues.Local.ToObservableCollection());
            context.Units.Load();
            tables.Add(context.Units.Local.ToObservableCollection());
            return tables;
        }
    }
}
