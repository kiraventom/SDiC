using SDiC.Authorization.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC
{
    class AuthorizationModel : IAuthorizationModel
    {
        public bool Login(Credentials credentials)
        {
            // TODO: Check DB
            return
                !string.IsNullOrWhiteSpace(credentials.Login) &&
                !string.IsNullOrWhiteSpace(credentials.Password);
        }
    }
}
