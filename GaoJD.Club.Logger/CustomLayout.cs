using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Logger
{
    public class CustomLayout : log4net.Layout.PatternLayout
    {
        public CustomLayout()
        {
            //应用ID
            this.AddConverter("ApplicationID", typeof(ApplicationIDPatternConverter));
            //错误消息
            this.AddConverter("Message", typeof(MessagePatternConverter));
            //调用方法
            this.AddConverter("Function", typeof(FunctionPatternConverter));
            //堆栈信息
            this.AddConverter("StackTrace", typeof(StackTracePatternConverter));
            //操作人
            this.AddConverter("LoginName", typeof(LoginNamePatternConverter));
            //创建时间
            this.AddConverter("CreateTime", typeof(CreateTimePatternConverter));
            //服务器名称
            this.AddConverter("ServerName", typeof(ServerNamePatternConverter));
            //客户端IP
            this.AddConverter("ClientIP", typeof(ClientIPPatternConverter));
            //请求URL
            this.AddConverter("PageUrl", typeof(PageUrlPatternConverter));
            //操作模块
            this.AddConverter("ModuleName", typeof(ModuleNamePatternConverter));
            //模块内方法名称
            this.AddConverter("MethodName", typeof(MethodNamePatternConverter));
            //操作对象ID
            this.AddConverter("TargetID", typeof(TargetIDPatternConverter));
            //动作
            this.AddConverter("Action", typeof(ActionPatternConverter));
            //组织机构ID
            this.AddConverter("OrganizationID", typeof(OrganizationIDPatternConverter));
            //日志类型
            this.AddConverter("ActionType", typeof(ActionTypePatternConverter));
            //来路地址
            this.AddConverter("ReferrerUrl", typeof(ReferrerUrlPatternConverter));

        }
    }
}
