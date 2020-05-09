using System;

namespace SDiC
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
