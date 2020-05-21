using App.Common.Abstraction;

namespace App.Main
{
    public sealed class MainModel : Model
    {
        public AuthorizationDB.User CurrentUser { get; set; }
    }
}
