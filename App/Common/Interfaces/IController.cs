using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC.Common
{
    public interface IController
    {
        void Show();
        void Close();
        IView View { get; }
        IModel Model { get; }
        event EventHandler<ControllerClosedEventArgs> ControllerClosed;
    }
}
