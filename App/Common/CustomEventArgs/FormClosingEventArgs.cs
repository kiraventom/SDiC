using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC.Common
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
