using App.Main.DbEdit.AddEntity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Main.DbEdit.AddEntity
{
    public class AddEntityModel : IAddEntityModel
    {
        public AddEntityModel(IEnumerable<string> logins)
        {
            Logins = logins.ToList();
        }

        private readonly IReadOnlyCollection<string> Logins;

        public Error CheckNewUser(Database.User user)
        {
            if (string.IsNullOrWhiteSpace(user.Login))
            {
                return Error.EmptyLogin;
            }
            else
            if (Logins.Any(login => login.Equals(user.Login, StringComparison.OrdinalIgnoreCase)))
            {
                return Error.ExistingLogin;
            }
            else
            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return Error.EmptyPassword;
            }
            else // все данные в порядке, можно добавлять аккаунт
            {
                return Error.None;
            }
        }

        public enum Error { None, EmptyLogin, ExistingLogin, EmptyPassword, EmptyType }
    }
}
