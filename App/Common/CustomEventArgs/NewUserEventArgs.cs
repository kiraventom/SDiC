using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.CustomEventArgs
{
    public class NewUserEventArgs : EventArgs
    {
        public NewUserEventArgs(Database.User user)
        {
            NewUser = user;
        }

        public Database.User NewUser { get; }
    }
}
