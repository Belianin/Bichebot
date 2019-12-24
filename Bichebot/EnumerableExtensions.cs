using System;
using System.Collections.Generic;
using System.Linq;

namespace Bichebot
{
    public static class EnumerableExtensions
    {
        public static bool ContainsAny(this string row, IEnumerable<string> strings)
        {
            return strings.Any(row.Contains);
        }

        public static T Random<T>(this IList<T> source, Random random)
        {
            return source[random.Next(0, source.Count - 1)];
        }
    }
}