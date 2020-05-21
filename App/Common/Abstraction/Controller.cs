using App.Common.CustomEventArgs;
using System;

namespace App.Common.Abstraction
{
    public abstract class Controller
    {
        public abstract void Show();
        public abstract void Close();
        protected abstract View View { get; }
        protected abstract Model Model { get; }
        public abstract event EventHandler<ControllerClosedEventArgs> ControllerClosed;
    }
}
