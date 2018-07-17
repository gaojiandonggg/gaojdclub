
using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Logic
{
    public interface IUserLogic : ILogicBase<User>
    {
        User GetUser(string userName, string userPwd);
    }
}
