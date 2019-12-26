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

        public static T RandomReadonly<T>(this IReadOnlyCollection<T> source, Random random)
        {
            return source.ElementAt(random.Next(0, source.Count - 1));
        }
        
        public static T Random<T>(this ICollection<T> source, Random random)
        {
            return source.ElementAt(random.Next(0, source.Count - 1));
        }
    }
}