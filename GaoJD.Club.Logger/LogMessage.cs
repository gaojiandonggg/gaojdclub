using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Logger
{
    public class LogMessage
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public int ApplicationID { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 调用方法
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 请求URL
        /// </summary>
        public string PageUrl { get; set; }

        //=================

        /// <summary>
        /// 操作模块
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块内方法名称
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 操作对象ID
        /// </summary>
        public string TargetID { get; set; }

        /// <summary>
        /// 动作
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 组织机构ID
        /// </summary>
        public int OrganizationID { get; set; }

        /// <summary>
        /// 日志记录类型
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 来路地址
        /// </summary>
        public string ReferrerUrl { get; set; }
    }
}
