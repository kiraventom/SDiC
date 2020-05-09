using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC
{
    public class Credentials
    {
        public Credentials(string login, string passwordHash)
        {
            Login = login;
            PasswordHash = passwordHash;
        }

        public string Login { get; }
        public string PasswordHash { get; }
    }
}
