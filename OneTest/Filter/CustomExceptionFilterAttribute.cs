using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaoJD.Club.Logger;
using Microsoft.AspNetCore.Mvc;
using GaoJD.Club.Core.Logger;

namespace GaoJD.Club.OneTest.Filter
{
    /// <summary>
    /// 异常处理过滤器-记录日志
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {

        private readonly ILogger _logger;
        private readonly ILogger<CustomExceptionFilterAttribute> logger;

        public CustomExceptionFilterAttribute(ILogger _logger, ILogger<CustomExceptionFilterAttribute> logger)
        {
            this._logger = _logger;
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;

            string t = context.Exception.GetType().FullName;

            JsonResult result = null;

            result = new JsonResult("服务器处理出错")
            {
                StatusCode = 500
            };
            _logger.Error(exception);
            context.Result = result;

        }
    }
}
