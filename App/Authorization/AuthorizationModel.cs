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
        public Database.User Login(Credentials credentials)
        {
            Database.User user = null;
            using (var db = new Database.UsersContext())
            {
                user = db.Users
                    .AsEnumerable()
                    .FirstOrDefault(u => credentials.Equals(u));
            }

            return user;
        }
    }
}
