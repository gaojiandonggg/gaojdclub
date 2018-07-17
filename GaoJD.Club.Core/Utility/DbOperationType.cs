using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GaoJD.Club.Core
{
    /// <summary>
    /// 数据库操作类型
    /// </summary>
    public enum DbOperationType
    {
        /// <summary>
        /// 插入操作
        /// </summary>
        [Description("插入操作")]
        INSERT = 0,
        /// <summary>
        /// 更新操作
        /// </summary>
        [Description("更新操作")]
        UPDATE = 1,
        /// <summary>
        /// 删除操作
        /// </summary>
        [Description("删除操作")]
        DELETE = 2,
        /// <summary>
        /// 查询操作
        /// </summary>
        [Description("查询操作")]
        SELECT = 3,
        /// <summary>
        /// COUNT操作
        /// </summary>
        [Description("COUNT操作")]
        COUNT = 4,
        /// <summary>
        /// EXISTS操作
        /// </summary>
        [Description("EXISTS操作")]
        EXISTS = 5
    }
}
