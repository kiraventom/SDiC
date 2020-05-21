using System;
using System.ComponentModel;
using System.Windows;

namespace App.Common.Abstraction
{
    public abstract class View
    {
        protected View()
        {
            Window.Closing += this.Window_Closing;
        }
        
        private void Window_Closing(object sender, CancelEventArgs e) => Closing.Invoke(this, e);
        public event EventHandler<CancelEventArgs> Closing;

        protected abstract Window Window { get; }

        public void Show() => Window.Show();
        public void Hide() => Window.Hide();
        public bool? ShowDialog() => Window.ShowDialog();
        public void Close() => Window.Close();
    }
}
