using Database;
using SDiC.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Main.DbEdit.AddEntity.Interfaces
{
    public interface IAddEntityModel : IModel
    {
        AddEntityModel.Error CheckNewUser(User user);
    }
}
