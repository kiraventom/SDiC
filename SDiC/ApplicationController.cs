using SDiC.Common;
using SDiC.Main;
using SDiC.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDiC
{
    public class ApplicationController
    {
        public ApplicationController()
        {
            Context = new ApplicationContext();
            AuthController = new AuthorizationController(new AuthorizationView(), new AuthorizationModel());
            AuthController.FormClosing += this.AuthController_FormClosing;
            MainController = new MainController(new MainView(), new MainModel());
            MainController.FormClosing += this.MainController_FormClosing;
        }

        private void AuthController_FormClosing(object sender, Common.FormClosingEventArgs e)
        {
            switch (e.Reason)
            {
                case Common.FormClosingEventArgs.CloseReason.Success: // sign in
                    Context.MainForm = MainController.View as Form;
                    AuthController.Close();
                    (MainController as IMainController).Login = (e.Data as Credentials).Login;
                    MainController.Show();
                    break;

                default:
                    throw new NotImplementedException(nameof(e.Reason));
            }
        }

        private void MainController_FormClosing(object sender, Common.FormClosingEventArgs e)
        {
            switch (e.Reason)
            {
                case Common.FormClosingEventArgs.CloseReason.Abort: // sign out
                    Context.MainForm = AuthController.View as Form;
                    MainController.Close();
                    AuthController.Show();
                    break;

                default:
                    throw new NotImplementedException(nameof(e.Reason));
            }
        }

        readonly IController AuthController;
        readonly IController MainController;
        ApplicationContext Context { get; set; }

        public void Run()
        {
            Context.MainForm = AuthController.View as Form;
            Application.Run(Context);
        }
    }
}
