using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GaoJD.Club.Core
{
    public static class EventExpression
    {

        public static void AddServiceEventBusExpression(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBus>();

            MapEventToHandler(services);
        }


        private static void MapEventToHandler(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                return;
            }
            foreach (var type in assembly.GetTypes())
            {

                var interfaces = type.GetInterfaces();

                if (typeof(IEventHandler).IsAssignableFrom(type))//判断当前类型是否实现了IEventHandler接口
                {
                    Type handlerInterface = type.GetInterface("IEventHandler`1");//获取该类实现的泛型接口
                    if (handlerInterface != null)
                    {
                        Type eventDataType = handlerInterface.GetGenericArguments()[0]; // 获取泛型接口指定的参数类型
                        services.AddSingleton(type);
                        EventBus.Default.Register(eventDataType, type);
                    }
                }
            }
        }

        private static void MapEventToHandlerAssembly(this IServiceCollection services, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {

                var interfaces = type.GetInterfaces();

                if (typeof(IEventHandler).IsAssignableFrom(type))//判断当前类型是否实现了IEventHandler接口
                {
                    Type handlerInterface = type.GetInterface("IEventHandler`1");//获取该类实现的泛型接口
                    if (handlerInterface != null)
                    {
                        Type eventDataType = handlerInterface.GetGenericArguments()[0]; // 获取泛型接口指定的参数类型
                        services.AddSingleton(type);
                        EventBus.Default.Register(eventDataType, type);
                    }
                }
            }
        }

    }
}
