using App.Authorization.Other;
using App.Common.Abstraction;
using System.Linq;

namespace App.Authorization
{
    public class AuthorizationModel : Model
    {
        public bool Login(Credentials credentials)
        {
            using (var db = new AuthorizationDB.UsersContext())
            {
                AuthorizedUser = db.Users
                    .AsEnumerable()
                    .FirstOrDefault(u => credentials.Equals(u));
            }
            return AuthorizedUser != null;
        }

        public AuthorizationDB.User AuthorizedUser { get; private set; } = null;
    }
}
