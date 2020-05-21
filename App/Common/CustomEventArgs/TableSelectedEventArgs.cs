using System;

namespace App.Common.CustomEventArgs
{
    public class TableSelectedEventArgs : EventArgs
    {
        public TableSelectedEventArgs(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
