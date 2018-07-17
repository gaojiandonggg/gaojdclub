using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Core;

namespace GaoJD.Club.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository()
        {

        }

        public User GetUser(string userName, string userPwd)
        {
            return GetItemByQuery(p => p.UserName == userName && p.Password == userPwd);
        }


    }
}
