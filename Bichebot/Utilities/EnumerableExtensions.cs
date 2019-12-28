using System;
using System.Collections.Generic;
using System.Linq;

namespace Bichebot.Utilities
{
    [Obsolete]
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
        
        public static bool ContainsAny(this string row, IEnumerable<string> strings)
        {
            return strings.Any(row.Contains);
        }
        
        public static T RandomReadonly<T>(this IReadOnlyCollection<T> source, Random random)
        {
            return source.ElementAt(random.Next(0, source.Count));
        }
        
        public static T Random<T>(this ICollection<T> source, Random random)
        {
            return random.Choose(source);
        }
        
        public static T Random<T>(this IEnumerable<T> source, Random random, Func<T, int> weight)
        {
            return random.Choose(source, weight);
        }
    }
}