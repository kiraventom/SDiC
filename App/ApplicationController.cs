using App.Authorization;
using App.Common.CustomEventArgs;
using App.Main;
using System;

namespace App
{
    public sealed class ApplicationController
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
                    MainController.CurrentUser = e.Data as AuthorizationDB.User;
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

        private AuthorizationController AuthController { get; }
        private MainController MainController { get; }

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
