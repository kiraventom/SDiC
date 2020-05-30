using System;

namespace App.Common.CustomEventArgs
{
    public class ParameterSelectionChangedEventArgs : EventArgs
    {
        public ParameterSelectionChangedEventArgs(string parameterType, string parameterName)
        {
            this.ParameterType = parameterType;
            this.ParameterName = parameterName;
        }

        public string ParameterType { get; }
        public string ParameterName { get; }
    }
}
