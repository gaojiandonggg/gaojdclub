using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace GaoJD.Club.Core
{
    public class FastProperty
    {
        /// <summary>
		/// 获取属性的值
		/// </summary>
		private Func<object, object> m_Getter;
        /// <summary>
        /// 设置属性的值
        /// </summary>
        private Action<object, object> m_Setter;
        /// <summary>
        /// 属性
        /// </summary>
        private PropertyInfo m_propertyInfo;
        /// <summary>
        /// 构造FastProperty
        /// </summary>
        /// <param name="property">属性</param>
        public FastProperty(PropertyInfo property)
        {
            m_propertyInfo = property;
            InitGetter();
            InitSetter();
        }

        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <param name="instance">实例对象</param>
        /// <returns></returns>
        public object Get(object instance)
        {
            return m_Getter(instance);
        }
        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void Set(object instance, object value)
        {
            m_Setter(instance, value);
        }
        /// <summary>
        /// 属性
        /// </summary>
        public PropertyInfo Property
        {
            get
            {
                return m_propertyInfo;
            }
        }
        /// <summary>
        /// 初始化Getter
        /// </summary>
        private void InitGetter()
        {
            ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "instance");
            //将instanceParameter直接转换成定义属性的对象
            var instanceCast = Expression.Convert(instanceParameter, m_propertyInfo.DeclaringType);
            MethodInfo method = m_propertyInfo.GetGetMethod();
            var methodCall = Expression.Call(instanceCast, method);
            //将返回值强制转换成object
            var methodCast = Expression.Convert(methodCall, typeof(object));
            var lambda = Expression.Lambda<Func<object, object>>(methodCast, instanceParameter);
            m_Getter = lambda.Compile();
        }
        /// <summary>
        /// 初始化Setter
        /// </summary>
        private void InitSetter()
        {
            ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "instance");
            ParameterExpression valueParameter = Expression.Parameter(typeof(object), "value");
            var instanceCast = Expression.Convert(instanceParameter, m_propertyInfo.DeclaringType);
            var valueCast = Expression.Convert(valueParameter, m_propertyInfo.PropertyType);
            MethodInfo method = m_propertyInfo.GetSetMethod();
            var methodCall = Expression.Call(instanceCast, method, valueCast);
            var lambda = Expression.Lambda<Action<object, object>>(methodCall, instanceParameter, valueParameter);
            m_Setter = lambda.Compile();
        }
    }
}
