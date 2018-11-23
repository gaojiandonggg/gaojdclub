using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core.Extensions
{
    public static class ListExtensions
    {
        public static bool IsFull<T>(this List<T> list)
        {
            return list.Count == list.Capacity;
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count == 0;
        }

        //public static List<T> To<T>(this List<Row> rows) where T : class
        //{
        //    var list = new List<T>();
        //    foreach (var item in rows)
        //    {
        //        list.Add(item.To<T>());
        //    }
        //    return list;
        //}
    }
}
