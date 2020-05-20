using Application;
using Application.Authorization;
using SDiC.Common;
using SDiC.Main;
using SDiC.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SDiC
{
    public class ApplicationController
    {
        public ApplicationController()
        {
            AuthController = new AuthorizationController(new AuthorizationView(), new AuthorizationModel());
            AuthController.ControllerClosed += this.AuthController_ControllerClosed;
            MainController = new MainController(new MainView(), new MainModel());
            MainController.ControllerClosed += this.MainController_ControllerClosed;
        }

        private void AuthController_ControllerClosed(object sender, ControllerClosedEventArgs e)
        {
            switch (e.Reason)
            {
                case ControllerClosedEventArgs.CloseReason.Success: // sign in
                    AuthController.Close();
                    (MainController as IMainController).CurrentUser = e.Data as AuthorizationDB.User;
                    MainController.Show();
                    break;

                case ControllerClosedEventArgs.CloseReason.Abort:
                    this.Stop();
                    break;

                default:
                    throw new NotImplementedException(nameof(e.Reason));
            }
        }

        private void MainController_ControllerClosed(object sender, ControllerClosedEventArgs e)
        {
            switch (e.Reason)
            {
                case ControllerClosedEventArgs.CloseReason.Abort: // sign out
                    MainController.Close();
                    AuthController.Show();
                    break;

                case ControllerClosedEventArgs.CloseReason.Success:
                    this.Stop();
                    break;

                default:
                    throw new NotImplementedException(nameof(e.Reason));
            }
        }

        readonly IController AuthController;
        readonly IController MainController;

        public void Run()
        {
            AuthController.Show();
        }

        private void Stop()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
