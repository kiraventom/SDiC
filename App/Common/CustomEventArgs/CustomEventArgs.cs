using System;

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
