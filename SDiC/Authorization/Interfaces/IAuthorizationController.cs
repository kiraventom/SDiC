using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC.Authorization.Interfaces
{
    public interface IAuthorizationController : IController
    {
        void View_LoginAttempt(object sender, LoginEventArgs e);
        void View_SuccessfulLogin(object sender, LoginEventArgs e);
    }
}
