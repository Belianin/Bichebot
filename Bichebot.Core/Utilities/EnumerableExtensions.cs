using System;
using System.Collections.Generic;
using System.Linq;

namespace Bichebot.Core.Utilities
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var e in source)
            {
                action(e);

                yield return e;
            }
        }
        
        public static bool ContainsAny(this string row, IEnumerable<string> rows)
        {
            return rows.Any(row.Contains);
        }
    }
}