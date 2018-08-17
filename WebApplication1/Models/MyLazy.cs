using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class MyLazy<T> where T : new()
    {
        private T value;
        private bool isLoaded;
        public MyLazy()
        {
            isLoaded = false;
        }
        public T Value
        {
            get
            {
                if (!isLoaded)
                {
                    value = new T();
                    isLoaded = true;
                }
                return value;
            }
        }
    }

    class Large
    {
        public Large() { }
        public void Test()
        {
            Console.WriteLine("Test");
        }
    }
}
