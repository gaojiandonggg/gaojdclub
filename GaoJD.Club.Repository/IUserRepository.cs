
using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Repository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        User GetUser(string userName, string userPwd);
    }
}
