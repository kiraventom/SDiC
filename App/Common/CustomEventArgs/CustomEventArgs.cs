using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.CustomEventArgs
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(object data)
        {
            Data = data;
        }
        public object Data;
    }
}
