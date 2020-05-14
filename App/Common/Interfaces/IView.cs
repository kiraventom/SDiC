using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SDiC.Common
{
    public interface IView
    {
        void Show();
        void Hide();
        bool? ShowDialog();
        void Close();
    }
}
