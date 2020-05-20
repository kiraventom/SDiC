using App.Main.DbEdit.Interfaces;
using SDiC.Common;
using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace App.Main.DbEdit
{
    public class ChemistryDbEditController : DbEditController
    {
        public ChemistryDbEditController(IDbEditView view, IDbEditModel model) : base(view, model)
        {
            CurrentType = typeof(ChemistryDB.Materials);
        }
    }
}
