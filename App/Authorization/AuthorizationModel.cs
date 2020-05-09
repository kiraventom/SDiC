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
            bool loginSuccessfull;
            using (var db = new Database.UsersContext())
            {
                var user = db.Users
                    .AsEnumerable()
                    .SingleOrDefault(u => u.Login.Equals(credentials.Login, StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    loginSuccessfull = user.PasswordHash.Equals(credentials.PasswordHash, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    loginSuccessfull = false;
                }
            }
            return loginSuccessfull;
        }
    }
}
