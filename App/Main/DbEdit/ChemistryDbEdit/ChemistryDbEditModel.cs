using App.Main.DbEdit.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace App.Main.DbEdit.ChemistryDbEdit
{
    public class ChemistryDbEditModel : IDbEditModel
    {
        public ChemistryDbEditModel()
        {
            Context = new ChemistryDB.ChemistryContext();
        }

        private readonly ChemistryDB.ChemistryContext Context;

        public dynamic ReadAll() => GetAllTables();

        private List<dynamic> GetAllTables()
        {
            List<dynamic> tables = new List<dynamic>();
            Context.Materials.Load();
            tables.Add(Context.Materials.Local.ToObservableCollection());
            Context.Parameters.Load();
            tables.Add(Context.Parameters.Local.ToObservableCollection());
            Context.ParametersTypes.Load();
            tables.Add(Context.ParametersTypes.Local.ToObservableCollection());
            Context.ParametersValues.Load();
            tables.Add(Context.ParametersValues.Local.ToObservableCollection());
            Context.Units.Load();
            tables.Add(Context.Units.Local.ToObservableCollection());
            return tables;
        }

        public void Save() => Context.SaveChanges();

        ~ChemistryDbEditModel() => Context.Dispose();
    }
}
