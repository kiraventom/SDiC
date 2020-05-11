using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC.Authorization.Interfaces
{
    public interface IAuthorizationModel : IModel
    {
        Database.User Login(Credentials credentials);
    }
}
