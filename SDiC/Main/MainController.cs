using SDiC.Common;
using SDiC.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDiC
{
    public class MainController : IMainController
    {
        public MainController(IMainView view, IMainModel model)
        {
            View = view;
            Model = model;

            View.SignOut += this.View_SignOut;
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
                FormClosing.Invoke(this, new Common.FormClosingEventArgs(Common.FormClosingEventArgs.CloseReason.Abort));
            }
        }

        public event EventHandler<Common.FormClosingEventArgs> FormClosing;

        public void Show()
        {
            View.Show();
        }

        public void Close() => View.Hide();
    }
}
