using App.Main.DbEdit;
using App.Main.DbEdit.ChemistryDbEdit;
using App.Main.DbEdit.Interfaces;
using SDiC.Common;
using SDiC.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SDiC
{
    public class MainController : IMainController
    {
        public MainController(IMainView view, IMainModel model)
        {
            View = view;
            Model = model;

            View.SignOut += this.View_SignOut;
            View.EditUsersDb += this.View_EditDb;
            View.EditChemistryDb += this.View_EditChemistryDb;
            (View as Window).Closed += this.MainView_Closed;
        }

        private void View_EditChemistryDb(object sender, EventArgs e)
        {
            DbEditController dbEditController = new ChemistryDbEditController(new ChemistryDbEditView(), new ChemistryDbEditModel());
            dbEditController.ControllerClosed += (sender, ea) => { };
            dbEditController.Show();
        }

        IView IController.View => View as IView;
        IModel IController.Model => Model as IModel;

        private readonly IMainView View;
        private readonly IMainModel Model;

        public AuthorizationDB.User CurrentUser
        {
            set
            {
                switch (value.Level)
                {
                    case 0:
                        View.Greeting = "исследователь";
                        View.IsEditDbBtsVisible = false;
                        break;
                    case 1:
                        View.Greeting = "администратор";
                        View.IsEditDbBtsVisible = true;
                        break;
                    default:
                        throw new NotImplementedException($"Unknown user level \"{value.Level}\"");
                }
                Model.CurrentUser = value;
            }
        }

        private void View_SignOut(object sender, EventArgs e)
        {
            bool shouldSignOut = View.ConfirmSigningOut();
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

        private void MainView_Closed(object sender, EventArgs e) 
        {
            ControllerClosed.Invoke(this, new ControllerClosedEventArgs(ControllerClosedEventArgs.CloseReason.Success));
        }

        public event EventHandler<ControllerClosedEventArgs> ControllerClosed;

        public void Show() => View.Show();
        public void Close() => View.Hide();
    }
}
