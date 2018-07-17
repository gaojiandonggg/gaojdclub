using GaoJD.Club.Utility;
using log4net;
using MicroKnights.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaoJD.Club.Logger
{
    public class Logger : ILogger
    {
        private ILog logger = LogManager.GetLogger("NETCoreRepository", typeof(Logger));

        private readonly AppConfigurtaionServices _AppConfigurtaionServices;
        public Logger(AppConfigurtaionServices AppConfigurtaionServices)
        {
            this._AppConfigurtaionServices = AppConfigurtaionServices;
        }

        #region 操作日志 - 通用
        /// <summary>
        /// 操作日志 - 通用
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="actionInfo"></param>
        public void Operate(string targetID, string actionInfo)
        {
            System.Reflection.MethodBase methodInfo = new System.Diagnostics.StackFrame(1).GetMethod();

            if (logger.IsInfoEnabled)
            {
                LogMessage cpm = new LogMessage
                {
                    ApplicationID = Convert.ToInt32(_AppConfigurtaionServices.GetAppSettings<AppSettings>("AppSettings").ApplicationID),
                    ModuleName = methodInfo.DeclaringType.FullName,
                    MethodName = methodInfo.Name,
                    TargetID = targetID,
                    Action = actionInfo,
                    LoginName = "",
                    CreateTime = DateTime.Now,
                    ServerName = UserHelper.GetServerName(),
                    ClientIP = UserHelper.GetUserIp(),
                    PageUrl = UserHelper.GetRequestUrl(),
                    OrganizationID = 43,
                    ActionType = 0,
                    ReferrerUrl = UserHelper.GetRequestUrlReferrerAbsoluteUri()
                };
                logger.Info(cpm);
            }
        }



        #endregion

        #region 操作日志-增加数据
        /// <summary>
        /// 操作日志-增加数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="entity">数据实体</param>
        public void AddOperate<T>(string targetID, T entity)
        {
            try
            {
                System.Reflection.MethodBase methodInfo = new System.Diagnostics.StackFrame(1).GetMethod();

                if (logger.IsInfoEnabled)
                {
                    LogMessage cpm = new LogMessage
                    {
                        ApplicationID = Convert.ToInt32(_AppConfigurtaionServices.GetAppSettings<AppSettings>("AppSettings").ApplicationID),
                        ModuleName = methodInfo.DeclaringType.FullName,
                        MethodName = methodInfo.Name,
                        TargetID = targetID,
                        Action = string.Format("添加操作：\r\n {0}", JsonConvert.SerializeObject(entity)),
                        LoginName = "",
                        CreateTime = DateTime.Now,
                        ServerName = UserHelper.GetServerName(),
                        ClientIP = UserHelper.GetUserIp(),
                        PageUrl = UserHelper.GetRequestUrl(),
                        OrganizationID = 43,
                        ActionType = 0,
                        ReferrerUrl = UserHelper.GetRequestUrlReferrerAbsoluteUri()
                    };
                    logger.Info(cpm);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region 操作日志 - 更新数据
        public void UpdateOperate<T>(string targetID, T oldEntity, T newEntity)
        {
            System.Reflection.MethodBase methodInfo = new System.Diagnostics.StackFrame(1).GetMethod();

            if (logger.IsInfoEnabled)
            {
                LogMessage cpm = new LogMessage
                {
                    ApplicationID = Convert.ToInt32(_AppConfigurtaionServices.GetAppSettings<AppSettings>("AppSettings").ApplicationID),
                    ModuleName = methodInfo.DeclaringType.FullName,
                    MethodName = methodInfo.Name,
                    TargetID = targetID,
                    Action = string.Format("更新数据：\r\n原始数据：\r\n {0} \r\n更新数据：\r\n {1}", JsonConvert.SerializeObject(oldEntity), JsonConvert.SerializeObject(newEntity)),
                    LoginName = "",
                    CreateTime = DateTime.Now,
                    ServerName = UserHelper.GetServerName(),
                    ClientIP = UserHelper.GetUserIp(),
                    PageUrl = UserHelper.GetRequestUrl(),
                    OrganizationID = 43,
                    ActionType = 0,
                    ReferrerUrl = UserHelper.GetRequestUrlReferrerAbsoluteUri()
                };
                logger.Info(cpm);
            }
        }
        #endregion

        #region 操作日志-删除数据
        /// <summary>
        /// 操作日志-删除数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="entity">数据实体</param>
        public void DeleteOperate<T>(string targetID, T entity)
        {
            System.Reflection.MethodBase methodInfo = new System.Diagnostics.StackFrame(1).GetMethod();

            if (logger.IsInfoEnabled)
            {
                LogMessage cpm = new LogMessage
                {
                    ApplicationID = Convert.ToInt32(_AppConfigurtaionServices.GetAppSettings<AppSettings>("AppSettings").ApplicationID),
                    ModuleName = methodInfo.DeclaringType.FullName,
                    MethodName = methodInfo.Name,
                    TargetID = targetID,
                    Action = string.Format("删除操作：\r\n {0}", JsonConvert.SerializeObject(entity)),
                    LoginName = "",
                    CreateTime = DateTime.Now,
                    ServerName = UserHelper.GetServerName(),
                    ClientIP = UserHelper.GetUserIp(),
                    PageUrl = UserHelper.GetRequestUrl(),
                    OrganizationID = 43,
                    ActionType = 0,
                    ReferrerUrl = UserHelper.GetRequestUrlReferrerAbsoluteUri()
                };
                logger.Info(cpm);
            }
        }
        #endregion

        #region 错误日志
        public void Error(Exception ex)
        {
            if (ex == null)
            {
                return;
            }
            Exception exInner = ex.InnerException;
            Exception exBase = ex.GetBaseException();

            if (exInner is System.Reflection.TargetInvocationException)//如果是：目标调用发生异常，取内部异常
            {
                if (exInner.InnerException != null)
                {
                    exInner = exInner.InnerException;
                }
            }
            /*
             *  为了保证所有的异常都显示最终消息，内部消息则记录数据库。
             *  例如：有些出错信息不应该提供给用户，但日志需要详细记录。
             *        则通过页面捕获异常后，包装此异常到新异常（新异常描述：系统出错！）内部，然后抛出。
             * */
            string msg = exInner != null ? exInner.Message : ex.Message;
            System.Reflection.MethodBase methodInfo = new System.Diagnostics.StackFrame(1).GetMethod();
            LogMessage errLog = new LogMessage()
            {
                ApplicationID = Convert.ToInt32(_AppConfigurtaionServices.GetAppSettings<AppSettings>("AppSettings").ApplicationID),
                Message = exBase.Message,
                Function = methodInfo.DeclaringType.FullName + "." + methodInfo.Name,
                StackTrace = exBase.StackTrace,
                LoginName = "",
                CreateTime = DateTime.Now,
                ServerName = UserHelper.GetServerName(),
                ClientIP = UserHelper.GetUserIp(),
                PageUrl = UserHelper.GetRequestUrl(),
                ActionType = 1,
                ReferrerUrl = UserHelper.GetRequestUrlReferrerAbsoluteUri()
            };
            //记录错误日志
            if (logger.IsErrorEnabled)
            {
                logger.Error(errLog);
            }
        }
        #endregion
    }
}
