using log4net.Core;
using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GaoJD.Club.Logger
{

    /// <summary>
    /// 应用ID
    /// </summary>
    internal sealed class ApplicationIDPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
            {
                writer.Write(logMessage.ApplicationID);
            }
        }
    }

    /// <summary>
    /// 错误消息
    /// </summary>
    internal sealed class MessagePatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
            {
                writer.Write(logMessage.Message);
            }
        }
    }

    /// <summary>
    /// 调用方法
    /// </summary>
    internal sealed class FunctionPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.Function);
        }
    }

    /// <summary>
    /// 堆栈信息
    /// </summary>
    internal sealed class StackTracePatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.StackTrace);
        }
    }

    /// <summary>
    /// 操作人
    /// </summary>
    internal sealed class LoginNamePatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.LoginName);
        }
    }

    /// <summary>
    /// 创建时间
    /// </summary>
    internal sealed class CreateTimePatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.CreateTime);
        }
    }

    /// <summary>
    /// 服务器名称
    /// </summary>
    internal sealed class ServerNamePatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.ServerName);
        }
    }

    /// <summary>
    /// 客户端IP
    /// </summary>
    internal sealed class ClientIPPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.ClientIP);
        }
    }

    /// <summary>
    /// 请求URL
    /// </summary>
    internal sealed class PageUrlPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.PageUrl);
        }
    }

    /// <summary>
    /// 操作模块
    /// </summary>
    internal sealed class ModuleNamePatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.ModuleName);
        }
    }

    /// <summary>
    /// 模块内方法名称
    /// </summary>
    internal sealed class MethodNamePatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.MethodName);
        }
    }

    /// <summary>
    /// 操作对象ID
    /// </summary>
    internal sealed class TargetIDPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.TargetID);
        }
    }

    /// <summary>
    /// 动作
    /// </summary>
    internal sealed class ActionPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.Action);
        }
    }

    /// <summary>
    /// 组织机构ID
    /// </summary>
    internal sealed class OrganizationIDPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.OrganizationID);
        }
    }

    /// <summary>
    /// 日志操作类型
    /// </summary>
    internal sealed class ActionTypePatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.ActionType);
        }
    }

    /// <summary>
    /// 来路地址
    /// </summary>
    internal sealed class ReferrerUrlPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.ReferrerUrl);
        }
    }



}
