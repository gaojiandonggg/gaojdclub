using System;
using System.Collections.Generic;
using System.Text;
using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Core;
using GaoJD.Club.Dto;
using GaoJD.Club.Repository;

namespace GaoJD.Club.Logic
{
    public class UserLogic : LogicBase<User>, IUserLogic
    {

        private IUserRepository _userRepository;
        public UserLogic(IRepositoryBase<User> dalBase, IUserRepository userRepository) : base(dalBase)
        {
            this._userRepository = userRepository;
        }
        public User GetUser(string userName, string userPwd)
        {

            return _userRepository.GetUser(userName, userPwd);
        }

        public string PostInput(User_Input user_Input)
        {
            return "";
        }
    }
}
