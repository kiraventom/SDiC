using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC.Main.Interfaces
{
    public interface IMainView : IView
    {
        event EventHandler SignOut;
        string Login { get; set; }
        bool ConfirmSigningOut();
    }
}
