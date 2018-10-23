using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core.Extensions
{
    /// <summary>
	/// 操作失败异常
	/// </summary>
	public class OperationFailedException : ApplicationException
    {
        /// <summary>
        /// 初始化 OperationFailedException
        /// </summary>
        public OperationFailedException()
            : base()
        {

        }
        /// <summary>
        /// 初始化 OperationFailedException
        /// </summary>
        /// <param name="message">错误消息</param>
        public OperationFailedException(string message)
            : base(message)
        {

        }
        /// <summary>
        /// 初始化 OperationFailedException
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="innerException">内部异常</param>
        public OperationFailedException(string message, Exception innerException) :
            base(message, innerException)
        {

        }
    }
}
