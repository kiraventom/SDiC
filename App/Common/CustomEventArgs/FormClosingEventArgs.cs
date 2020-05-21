using System;

namespace App.Common.CustomEventArgs
{
    public class ControllerClosedEventArgs : EventArgs
    {
        public ControllerClosedEventArgs(CloseReason closeReason)
        {
            Reason = closeReason;
            Data = null;
        }

        public ControllerClosedEventArgs(CloseReason closeReason, object data)
        {
            Reason = closeReason;
            Data = data;
        }

        public enum CloseReason { Success, Fail, Abort }

        public CloseReason Reason;
        public object Data;
    }
}
