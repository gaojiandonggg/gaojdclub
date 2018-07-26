using GaoJD.Club.Utility;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GaoJD.Club.OneTest.Extensions
{

    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return HttpContext.Current.Request.Cookies["woshiceshi"]?.ToString();
            // return connection.GetHttpContext().Request.Cookies["woshiceshi"]?.ToString();
            // return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
