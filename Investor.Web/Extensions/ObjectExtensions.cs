using System;

namespace Investor.Extensions
{
    public static class ObjectExtensions
    {
        public static T ChangeType<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}