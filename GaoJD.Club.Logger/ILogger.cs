using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Logger
{
    public interface ILogger
    {

        #region 操作日志 - 通用
        /// <summary>
        /// 操作日志 - 通用
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="actionInfo"></param>
        void Operate(string targetID, string actionInfo);
        #endregion

        #region 操作日志-增加数据
        /// <summary>
        /// 操作日志-增加数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="entity">数据实体</param>
        void AddOperate<T>(string targetID, T entity);
        #endregion

        #region 操作日志 - 更新数据
        void UpdateOperate<T>(string targetID, T oldEntity, T newEntity);
        #endregion

        #region 操作日志-删除数据
        /// <summary>
        /// 操作日志-删除数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="entity">数据实体</param>
        void DeleteOperate<T>(string targetID, T entity);
        #endregion

        #region 错误日志
        void Error(Exception ex);
        #endregion
    }
}
