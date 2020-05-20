using SDiC.Authorization.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC
{
    public class Credentials
    {
        public Credentials(string login, string password)
        {
            Login = login;
            PasswordHash = Hasher.GetHash(password);
        }

        public string Login { get; }
        public string PasswordHash { get; }

        public override bool Equals(object obj)
        {
            return
                obj is Credentials credentials
                &&
                this.Login.Equals(credentials.Login, StringComparison.OrdinalIgnoreCase)
                &&
                this.PasswordHash.Equals(credentials.PasswordHash, StringComparison.OrdinalIgnoreCase);
        }

        public bool Equals(AuthorizationDB.User user)
        {
            return
                this.Login.Equals(user.Login, StringComparison.OrdinalIgnoreCase)
                &&
                this.PasswordHash.Equals(user.PasswordHash, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode() => HashCode.Combine(this.Login, this.PasswordHash);
    }
}
