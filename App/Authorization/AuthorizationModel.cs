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
            using (var db = new Database.UsersContext())
            {
                AuthorizedUser = db.Users
                    .AsEnumerable()
                    .FirstOrDefault(u => credentials.Equals(u));
            }

            return AuthorizedUser is null;
        }

        public Database.User AuthorizedUser { get; private set; } = null;
    }
}
