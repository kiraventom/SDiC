using SDiC.Authorization.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.CustomEventArgs
{
    public class NewUserEventArgs : EventArgs
    {
        public NewUserEventArgs(string login, string password, int level)
        {
            NewUser = new Database.User() { Login = login, PasswordHash = Hasher.GetHash(password), Level = level };
        }

        public Database.User NewUser { get; }
    }
}
