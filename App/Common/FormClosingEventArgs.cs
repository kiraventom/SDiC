using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC.Common
{
    public class WindowClosingEventArgs : EventArgs
    {
        public WindowClosingEventArgs(CloseReason closeReason)
        {
            Reason = closeReason;
            Data = null;
        }

        public WindowClosingEventArgs(CloseReason closeReason, object data)
        {
            Reason = closeReason;
            Data = data;
        }

        public enum CloseReason { Success, Fail, Abort }

        public CloseReason Reason;
        public object Data;
    }
}
