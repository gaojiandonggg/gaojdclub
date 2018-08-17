using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GaoJD.Club.OneTest.Filter
{
    public class ModelValidateFilter : ActionFilterAttribute
    {
        public ModelValidateFilter()
        {

        }



        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var result = context.ModelState;
            //说明有的验证没有通过
            if (result.ErrorCount > 0)
            {
                var errors = context.ModelState.Values.SelectMany(p => p.Errors);
                //IEnumerable<ModelErrorCollection> errors1 = context.ModelState.Values.Select(p => p.Errors);
                //var exceptions = errors1.SelectMany(p => p.Select(x => x.Exception));

                var exceptions = errors.Select(p => p.Exception);

                var errorMessage = errors.Select(p => p.ErrorMessage);


                foreach (var item in result)
                {
                    var aaa = item.Value;
                }
            }

            // context.Result = new ObjectResult("不能为空");
        }
    }
}
