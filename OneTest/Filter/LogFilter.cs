﻿using GaoJD.Club.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GaoJD.Club.OneTest.Filter
{
    public class LogFilter : ActionFilterAttribute
    {

        Stopwatch sw;
        private readonly ILogger logger;

        public LogFilter(ILogger logger)
        {
            this.logger = logger;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            sw = new Stopwatch();
            sw.Start();
            var aaa = context.ActionArguments;
            if (aaa != null && aaa.Count > 0)
            {
                string json = JsonConvert.SerializeObject(aaa);
                logger.Operate("Input", json);
            }

            base.OnActionExecuting(context);

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var bbb = context.Result;
            if (bbb != null && bbb.GetType() == typeof(ObjectResult))
            {
                logger.Operate("OutPut", JsonConvert.SerializeObject(((ObjectResult)bbb).Value));
            }
            sw.Stop();
            long aaa = sw.ElapsedMilliseconds;

            base.OnActionExecuted(context);
        }


    }
}
