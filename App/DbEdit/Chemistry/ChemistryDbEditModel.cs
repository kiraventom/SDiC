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
            // TODO: use reflection
            List<dynamic> tables = new List<dynamic>();

            context.Material.Load();
            tables.Add(context.Material.Local.ToObservableCollection());

            context.Parameter.Load();
            tables.Add(context.Parameter.Local.ToObservableCollection());

            context.ParameterType.Load();
            tables.Add(context.ParameterType.Local.ToObservableCollection());

            context.ParameterValue.Load();
            tables.Add(context.ParameterValue.Local.ToObservableCollection());

            context.Unit.Load();
            tables.Add(context.Unit.Local.ToObservableCollection());

            return tables;
        }
    }
}
