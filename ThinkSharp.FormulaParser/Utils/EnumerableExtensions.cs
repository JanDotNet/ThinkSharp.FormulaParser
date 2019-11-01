using System;
using System.Collections.Generic;
using System.Text;

namespace ThinkSharp.FormulaParsing.Utils
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> WrapFirst<T>(this IEnumerable<T> items, Func<T, T> projection)
        {
            bool isFirst = true;
            foreach(var item in items)
            {
                yield return isFirst ? projection(item) : item;
                isFirst = false;
            }
        }
    }
}
