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
            AuthController.WindowClosed += this.AuthController_WindowClosed;
            MainController = new MainController(new MainView(), new MainModel());
            MainController.WindowClosed += this.MainController_WindowClosing;
        }

        private void AuthController_WindowClosed(object sender, WindowClosingEventArgs e)
        {
            switch (e.Reason)
            {
                case WindowClosingEventArgs.CloseReason.Success: // sign in
                    AuthController.Close();
                    (MainController as IMainController).CurrentUser = e.Data as Database.User;
                    MainController.Show();
                    break;

                case WindowClosingEventArgs.CloseReason.Abort:
                    this.Stop();
                    break;

                default:
                    throw new NotImplementedException(nameof(e.Reason));
            }
        }

        private void MainController_WindowClosing(object sender, Common.WindowClosingEventArgs e)
        {
            switch (e.Reason)
            {
                case WindowClosingEventArgs.CloseReason.Abort: // sign out
                    MainController.Close();
                    AuthController.Show();
                    break;

                case WindowClosingEventArgs.CloseReason.Success:
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
