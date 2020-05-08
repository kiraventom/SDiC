using SDiC.Common;
using SDiC.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDiC.Main
{
    public partial class MainView : Form, IMainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void SignOutBt_Click(object sender, EventArgs e)
        {
            SignOut.Invoke(this, EventArgs.Empty);
        }

        public bool ConfirmSigningOut()
        {
            var dr = MessageBox.Show(text: "Вы действительно хотите выйти из аккаунта?",
                                    caption: "Подтверждение",
                                    buttons: MessageBoxButtons.YesNo,
                                    icon: MessageBoxIcon.Question);
            return dr == DialogResult.Yes;
        }

        public event EventHandler SignOut;

        const string greeting = "Здравствуйте, ";
        private string login;
        public string Login 
        { 
            get => login;
            set
            {
                login = value;
                GreetingsL.Text = greeting + login;
            }
        }
    }
}
