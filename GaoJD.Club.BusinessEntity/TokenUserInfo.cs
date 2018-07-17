using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.BusinessEntity
{
    public class TokenUserInfo
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserLogo { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string RandomStr { get; set; }
    }
}
