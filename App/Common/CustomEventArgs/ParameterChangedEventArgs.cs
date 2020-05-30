using System;

namespace App.Common.CustomEventArgs
{
    public class ParameterChangedEventArgs : EventArgs
    {
        public ParameterChangedEventArgs(string parameterType, string parameterName, double value)
        {
            this.ParameterType = parameterType;
            this.ParameterName = parameterName;
            this.Value = value;
        }

        public string ParameterType { get; }
        public string ParameterName { get; }
        public double Value { get; }
    }
}
