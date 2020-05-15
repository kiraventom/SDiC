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
        event EventHandler EditDb;
        string Greeting { set; }
        bool ConfirmSigningOut();
        bool IsEditDbBtVisible { set; }
    }
}
