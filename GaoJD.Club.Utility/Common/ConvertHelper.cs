using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace GaoJD.Club.Utility
{
    public class ConvertHelper
    {
        #region 将datatable转换为list
        /// <summary>
        /// 将datatable转换为list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertDTToList<T>(DataTable dt) where T : class
        {
            Type t = typeof(T);
            //查询实例公共属性
            PropertyInfo[] properties = t.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            List<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo p in properties)
                {
                    object value = row[p.Name];
                    //排除DBNull
                    if (value != DBNull.Value)
                    {
                        p.SetValue(obj, value, null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        #endregion

        #region 将list转换为datatable
        /// <summary>
        /// 将list转换为datatable-支持int指针类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public unsafe static DataTable ConvertListToDT<T>(List<T> list) where T : class
        {
            Type t = typeof(T);
            DataTable dt = new DataTable(t.Name);
            PropertyInfo[] properties = t.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo p in properties)
            {
                Type colType = p.PropertyType;
                if (colType.IsPointer)
                {//如果是指针类型
                    switch (p.PropertyType.ToString())
                    {
                        case "System.Int32*":
                            colType = typeof(Int32);
                            break;
                            //可以拓展其它指针类型
                    }
                }
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {//如果是Nullable<T>类型
                    colType = colType.GetGenericArguments()[0];
                }
                dt.Columns.Add(p.Name, colType);
            }
            foreach (T obj in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo p in properties)
                {
                    switch (p.PropertyType.ToString())
                    {
                        case "System.Int32*":
                            //System.Reflection.Pointer类型,指针的托管表示
                            object objPtr = p.GetValue(obj, null);
                            //返回指针类型
                            int* ptr = (int*)Pointer.Unbox(objPtr);
                            row[p.Name] = *ptr;
                            break;
                        //可以拓展其它指针类型
                        default:
                            object obj2 = p.GetValue(obj, null);
                            row[p.Name] = obj2 == null ? DBNull.Value : obj2;
                            break;
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        #endregion
    }
}
