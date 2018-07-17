using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Utility
{
    /// <summary>
    /// 映射对象的属性信息
    /// </summary>
    public class MapperPropertyInfo
    {
        /// <summary>
        /// 是不是主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// 是否是自增的列
        /// </summary>
        public bool IsIdentity { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性的值
        /// </summary>
        public object Value { get; set; }
    }
}
