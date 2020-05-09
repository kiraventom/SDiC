using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC.Authorization.Interfaces
{
    public interface IAuthorizationView : IView
    {
        event EventHandler<LoginEventArgs> LoginAttempt;
        event EventHandler<LoginEventArgs> SuccessfulLogin;
        void LoginAttemptResult(bool credentialsOK);
    }
}
