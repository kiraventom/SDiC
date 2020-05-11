using SDiC.Common;
using SDiC.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC
{
    public class MainModel : IMainModel
    {
        public Database.User CurrentUser { get; set; }
    }
}
