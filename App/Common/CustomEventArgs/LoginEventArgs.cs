using App.Authorization.Other;
using System;

namespace App.Common.CustomEventArgs
{
    public class LoginEventArgs : EventArgs
    {
        public LoginEventArgs(Credentials credentials)
        {
            Credentials = credentials;
        }

        public Credentials Credentials { get; }
    }
}
