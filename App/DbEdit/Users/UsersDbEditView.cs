using App.Authorization.Other;
using App.DbEdit.Abstraction;
using System.Windows;

namespace App.DbEdit.Users
{
    public class UsersDbEditView : DbEditView
    {
        public UsersDbEditView() : base()
        {
            usersWindow.GenerateHashBt.Click += GenerateHashBt_Click;
        }

        protected override Window Window => usersWindow;
        private readonly UsersDbEditWindow usersWindow = new UsersDbEditWindow();

        private void GenerateHashBt_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Move to model
            var hash = 
                string.IsNullOrWhiteSpace(usersWindow.HasherPB.Password) 
                ? string.Empty 
                : Hasher.GetHash(usersWindow.HasherPB.Password);
            var cb = new TextCopy.Clipboard();
            cb.SetText(hash);
            usersWindow.ConfirmPU.IsOpen = true;
        }
    }
}
