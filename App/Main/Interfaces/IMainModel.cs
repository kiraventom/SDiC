using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC.Main.Interfaces
{
    public interface IMainModel : IModel
    {
        AuthorizationDB.User CurrentUser { get; set; }
    }
}
