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
                db.Add(new Database.User { Login = "admin", PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8" });
                db.SaveChanges();
            }
            return
                !string.IsNullOrWhiteSpace(credentials.Login) &&
                !string.IsNullOrWhiteSpace(credentials.PasswordHash);
        }
    }
}
