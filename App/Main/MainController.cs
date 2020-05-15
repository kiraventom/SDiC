using App.Main.DbEdit;
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
            View.EditDb += this.View_EditDb;
            (View as Window).Closed += this.MainView_Closed;
        }

        IView IController.View => View as IView;
        IModel IController.Model => Model as IModel;

        private readonly IMainView View;
        private readonly IMainModel Model;

        public Database.User CurrentUser
        {
            set
            {
                switch (value.Level)
                {
                    case 0:
                        View.Greeting = "исследователь";
                        View.IsEditDbBtVisible = false;
                        break;
                    case 1:
                        View.Greeting = "администратор";
                        View.IsEditDbBtVisible = true;
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
            IDbEditController dbEditController = new DbEditController(new DbEditView(), new DbEditModel());
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
