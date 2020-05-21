using App.DbEdit.Abstraction;

namespace App.DbEdit.Users
{
    public class UsersDbEditController : DbEditController
    {
        public UsersDbEditController(DbEditView view, DbEditModel model) : base(view, model)
        {
            CurrentType = typeof(AuthorizationDB.User);
        }
    }
}
