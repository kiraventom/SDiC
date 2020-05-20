using System;

namespace SDiC
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
