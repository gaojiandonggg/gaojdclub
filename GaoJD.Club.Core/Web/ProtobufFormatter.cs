using GaoJD.Club.Core.Extensions;
using GaoJD.Club.Core.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GaoJD.Club.Core
{
    public class ProtobufFormatter : OutputFormatter
    {
        public string ContentType { get; private set; }
        public ProtobufFormatter()
        {
            ContentType = "application/proto";
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ContentType));
        }

        /// <summary>
		/// 能否使用当前格式输出
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            Assert.NotNull(context, nameof(context));
            if (!context.ContentType.HasValue)
            {
                return false;
            }
            var parsedContentType = new MediaType(context.ContentType);
            for (var i = 0; i < SupportedMediaTypes.Count; i++)
            {
                var supportedMediaType = new MediaType(SupportedMediaTypes[i]);
                if (supportedMediaType.IsSubsetOf(parsedContentType)
                        &&
                         context.ObjectType.GetCustomAttribute(typeof(ProtoContractAttribute)
                          ) != null
                        )
                {
                    context.ContentType = new StringSegment(SupportedMediaTypes[i]);
                    return true;
                }
            }
            return false;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
            {
                throw new OperationFailedException("context不能为空");
            }
            HttpResponse httpResponse = context.HttpContext.Response;
            ProtobufUtils.Serialize(httpResponse.Body, context.Object);
            return Task.FromResult(0);
        }
    }
}
