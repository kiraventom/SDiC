using SDiC.Common;
using SDiC.Main.Interfaces;
using System;
using System.Collections.Generic;
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
            (View as Window).Closed += this.MainController_Closed;
        }


        IView IController.View => View as IView;
        IModel IController.Model => Model as IModel;

        public IMainView View;
        public IMainModel Model;

        public string Login 
        { 
            set => View.Login = value;
        }

        private void View_SignOut(object sender, EventArgs e)
        {
            bool shouldSignOut = View.ConfirmSigningOut();
            if (shouldSignOut)
            {
                WindowClosing.Invoke(this, new WindowClosingEventArgs(WindowClosingEventArgs.CloseReason.Abort));
            }
        }

        private void MainController_Closed(object sender, EventArgs e) 
        {
            WindowClosing.Invoke(this, new WindowClosingEventArgs(WindowClosingEventArgs.CloseReason.Success));
        }

        public event EventHandler<WindowClosingEventArgs> WindowClosing;

        public void Show()
        {
            View.Show();
        }

        public void Close() => View.Hide();
    }
}
