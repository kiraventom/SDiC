using App.Common.Abstraction;
using App.Common.CustomEventArgs;
using App.DbEdit.Abstraction;
using App.DbEdit.Chemistry;
using App.DbEdit.Users;
using System;
using System.ComponentModel;

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
        }

        private void View_EditChemistryDb(object sender, EventArgs e)
        {
            DbEditController dbEditController = new ChemistryDbEditController(new ChemistryDbEditView(), new ChemistryDbEditModel());
            dbEditController.ControllerClosed += (sender, ea) => { };
            dbEditController.Show();
        }

        protected override View View => view as View;
        protected override Model Model => model as Model;

        private readonly MainView view;
        private readonly MainModel model;

        public AuthorizationDB.User CurrentUser
        {
            set
            {
                switch (value.Level)
                {
                    case 0:
                        view.Greeting = "исследователь";
                        view.IsEditDbBtsVisible = false;
                        break;
                    case 1:
                        view.Greeting = "администратор";
                        view.IsEditDbBtsVisible = true;
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
