
using GaoJD.Club.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace GaoJD.Club.Core
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly IEventStore _eventStore;

        public static EventBus Default { get; private set; }


        public EventBus()
        {
            _eventStore = new InMemoryEventStore();
        }

        static EventBus()
        {
            Default = new EventBus();
        }

        #region Register

        /// <summary>
        /// 手动绑定事件源与事件处理
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventHandler"></param>
        public void Register<TEventData>(IEventHandler eventHandler) where TEventData : IEventData
        {
            Register(typeof(TEventData), eventHandler.GetType());
        }

        /// <summary>
        /// 注册Action事件处理器
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="action"></param>
        public void Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            //1.构造ActionEventHandler
            var actionHandler = new ActionEventHandler<TEventData>(action);

            //2.将ActionEventHandler的实例注入到Ioc容器
            //IocContainer.Register(
            //    Component.For<IEventHandler<TEventData>>()
            //    .UsingFactoryMethod(() => actionHandler));
            
            //3.注册到事件总线
            Register<TEventData>(actionHandler);
        }

        /// <summary>
        /// 手动绑定事件源与事件处理
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handlerType"></param>
        public void Register(Type eventType, Type handlerType)
        {
            //注册IEventHandler<T>到IOC容器
            _eventStore.AddRegister(eventType, handlerType);
        }

        #endregion

        #region UnRegister 

        /// <summary>
        /// 手动解除事件源与事件处理的绑定
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="handlerType"></param>
        public void UnRegister<TEventData>(Type handlerType) where TEventData : IEventData
        {
            _eventStore.RemoveRegister(typeof(TEventData), handlerType);
        }

        /// <summary>
        /// 卸载指定事件源上绑定的所有事件
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        public void UnRegisterAll<TEventData>() where TEventData : IEventData
        {
            //获取所有映射的EventHandler
            List<Type> handlerTypes = _eventStore.GetHandlersForEvent(typeof(TEventData)).ToList();
            foreach (var handlerType in handlerTypes)
            {
                _eventStore.RemoveRegister(typeof(TEventData), handlerType);
            }
        }

        #endregion

        #region Trigger

        /// <summary>
        /// 根据事件源触发绑定的事件处理
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventData"></param>
        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            //获取所有映射的EventHandler
            List<Type> handlerTypes = _eventStore.GetHandlersForEvent(eventData.GetType()).ToList();

            if (handlerTypes.Count > 0)
            {
                foreach (var handlerType in handlerTypes)
                {
                    IEventHandler<TEventData> eventHandler = (IEventHandler<TEventData>)HttpContext.Current.RequestServices.GetService(handlerType);
                    eventHandler?.HandleEvent(eventData);
                }
            }
        }

        /// <summary>
        /// 触发指定EventHandler
        /// </summary>
        /// <param name="eventHandlerType"></param>
        /// <param name="eventData"></param>

        public void Trigger<TEventData>(Type eventHandlerType, TEventData eventData)
            where TEventData : IEventData
        {
            if (_eventStore.HasRegisterForEvent<TEventData>())
            {
                var handlers = _eventStore.GetHandlersForEvent<TEventData>();
                if (handlers.Any(th => th == eventHandlerType))
                {
                    var type = handlers.Where(p => p == eventHandlerType).FirstOrDefault();
                    IEventHandler<TEventData> eventHandler = (IEventHandler<TEventData>)HttpContext.Current.RequestServices.GetService(type);
                    eventHandler?.HandleEvent(eventData);

                }
            }
        }

        /// <summary>
        /// 异步触发
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return Task.Run(() => Trigger<TEventData>(eventData));
        }

        /// <summary>
        /// 异步触发指定EventHandler
        /// </summary>
        /// <param name="eventHandlerType"></param>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public Task TriggerAsycn<TEventData>(Type eventHandlerType, TEventData eventData)
            where TEventData : IEventData
        {
            return Task.Run(() => Trigger(eventHandlerType, eventData));
        }


        /// <summary>
        /// 提供入口支持注册其它程序集中实现的IEventHandler
        /// </summary>
        /// <param name="assembly"></param>
        public void RegisterAllEventHandlerFromAssembly(Assembly assembly)
        {
            //1.将IEventHandler注册到Ioc容器
            //IocContainer.Register(Classes.FromAssembly(assembly)
            //    .BasedOn(typeof(IEventHandler<>))
            //    .WithService.Base());

            ////2.从IOC容器中获取注册的所有IEventHandler
            //var handlers = IocContainer.Kernel.GetAssignableHandlers(typeof(IEventHandler));
            //foreach (var handler in handlers)
            //{
            //    //循环遍历所有的IEventHandler<T>
            //    var interfaces = handler.ComponentModel.Implementation.GetInterfaces();
            //    foreach (var @interface in interfaces)
            //    {
            //        if (!typeof(IEventHandler).IsAssignableFrom(@interface))
            //        {
            //            continue;
            //        }

            //        //获取泛型参数类型
            //        var genericArgs = @interface.GetGenericArguments();
            //        if (genericArgs.Length == 1)
            //        {
            //            //注册到事件源与事件处理的映射字典中
            //            Register(genericArgs[0], handler.ComponentModel.Implementation);
            //        }
            //    }
            //}
        }

        #endregion
    }
}
