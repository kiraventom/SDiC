using App.Common.Abstraction;
using App.Common.CustomEventArgs;
using App.DbEdit.Abstraction;
using App.DbEdit.Chemistry;
using App.DbEdit.Users;
using App.ResultsTable;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace App.Main
{
    public sealed class MainController : Controller
    {
        public MainController(MainView view, MainModel model)
        {
            this.view = view;
            this.model = model;

            this.view.SignOut += this.View_SignOut;
            this.view.EditUsersDb += this.View_EditDb;
            this.view.EditChemistryDb += this.View_EditChemistryDb;
            this.view.Closing += this.MainView_Closing;
            this.view.WindowLoaded += this.View_WindowLoaded;
            this.view.MaterialChanged += this.View_MaterialChanged;
            this.view.ParameterChanged += this.View_ParameterChanged;
            this.view.SolveRequest += this.View_SolveRequest;
            this.view.SaveReportRequest += this.View_SaveReportRequest;
            this.view.ShowResultTableRequest += this.View_ShowResultTableRequest;
        }

        protected override View View => view;
        protected override Common.Abstraction.Model Model => model;

        private readonly MainView view;
        private readonly MainModel model;

        private void View_WindowLoaded(object sender, EventArgs e)
        {
            view.Materials = model.GetMaterialsNames();
        }

        private void View_MaterialChanged(object sender, CustomEventArgs e)
        {
            var name = e.Data.ToString();
            model.SetSelectedMaterial(name);
        }

        private void View_ParameterChanged(object sender, ParameterChangedEventArgs e)
        {
            model.SetParameterValue(e.ParameterType, e.ParameterName, e.Value);
        }

        private void View_ShowResultTableRequest(object sender, EventArgs e)
        {
            // TODO: consider move to model (do not forget do the same in dbedit)
            if (this.model.Solution != null)
            {
                var rtView = new ResultsTableView();
                var rtModel = new ResultsTableModel(model.Solution.CoordinateByChannelLength, model.Solution.Temperature, model.Solution.Viscosity);
                var rtController = new ResultsTableController(rtView, rtModel);
                rtController.ControllerClosed += (s, ea) => { };
                rtController.Show();
            }
            else
            {
                MainView.ShowOutputErrorMessage();
            }
        }

        private void View_SolveRequest(object sender, EventArgs e)
        {
            var solution = model.GetSolution();
            view.SetOutputValues(solution);
            var etaPoints = new List<DataPoint>();
            var TPoints = new List<DataPoint>();
            for (int i = 0; i < solution.CoordinateByChannelLength.Count; ++i)
            {
                etaPoints.Add(new DataPoint(solution.CoordinateByChannelLength.ElementAt(i), solution.Viscosity.ElementAt(i)));
                TPoints.Add(new DataPoint(solution.CoordinateByChannelLength.ElementAt(i), solution.Temperature.ElementAt(i)));
            }
            view.SetOutputCharts(etaPoints, TPoints);
        }

        private void View_SaveReportRequest(object sender, EventArgs e)
        {
            if (this.model.Solution != null)
            {
                var sfd = new Microsoft.Win32.SaveFileDialog()
                {
                    Filter = "XLSX файл |*.xlsx",
                    AddExtension = true,
                    DefaultExt = "xlsx"
                };
                var result = sfd.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    model.SaveReport(sfd.FileName);
                }
            }
            else
            {
                MainView.ShowOutputErrorMessage();
            }
        }

        private void View_EditChemistryDb(object sender, EventArgs e)
        {
            DbEditController dbEditController = new ChemistryDbEditController(new ChemistryDbEditView(), new ChemistryDbEditModel());
            dbEditController.ControllerClosed += (sender, ea) => { };
            dbEditController.Show();
        }

        public AuthorizationDB.User CurrentUser
        {
            set
            {
                switch (value.Level)
                {
                    case 0:
                        view.Greeting = "исследователь";
                        view.IsAdmin = false;
                        break;
                    case 1:
                        view.Greeting = "администратор";
                        view.IsAdmin = true;
                        break;
                    default:
                        throw new NotImplementedException($"Unknown user level \"{value.Level}\"");
                }
                model.CurrentUser = value;
            }
        }

        private void View_SignOut(object sender, EventArgs e)
        {
            bool shouldSignOut = MainView.ConfirmSigningOut();
            if (shouldSignOut)
            {
                ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Abort));
            }
        }

        private void View_EditDb(object sender, EventArgs e)
        {
            DbEditController dbEditController = new UsersDbEditController(new UsersDbEditView(), new UsersDbEditModel());
            dbEditController.ControllerClosed += (sender, ea) => { };
            dbEditController.Show();
        }

        private void MainView_Closing(object sender, CancelEventArgs e) 
        {
            ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Success));
        }

        public override event EventHandler<ControllerClosedEventArgs> ControllerClosed;

        public override void Show() => view.Show();
        public override void Close() => view.Hide();
    }
}
