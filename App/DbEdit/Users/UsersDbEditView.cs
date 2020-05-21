using App.Authorization.Other;
using App.DbEdit.Abstraction;
using System.Windows;

namespace App.DbEdit.Users
{
    public class UsersDbEditView : DbEditView
    {
        public UsersDbEditView() : base()
        {
            window.GenerateHashBt.Click += GenerateHashBt_Click;
        }

        protected override Window Window => window;
        private readonly UsersDbEditWindow window = new UsersDbEditWindow();

        private void GenerateHashBt_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Move to model
            var hash = 
                string.IsNullOrWhiteSpace(window.HasherPB.Password) 
                ? string.Empty 
                : Hasher.GetHash(window.HasherPB.Password);
            TextCopy.Clipboard.SetText(hash);
            window.ConfirmPU.IsOpen = true;
        }
    }
}
