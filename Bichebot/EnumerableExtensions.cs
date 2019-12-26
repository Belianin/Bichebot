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
            return source.ElementAt(random.Next(0, source.Count));
        }
        
        public static T Random<T>(this ICollection<T> source, Random random)
        {
            return source.ElementAt(random.Next(0, source.Count - 1));
        }
        
        public static T Random<T>(this IEnumerable<T> source, Random random, Func<T, int> weight)
        {
            var rates = new Dictionary<int, T>();
            var sum = 0;
            foreach (var c in source)
            {
                sum += weight(c);
                rates[sum] = c;
            }
            
            var rate = random.Next(0, sum);
            return rates.First(k => k.Key >= rate).Value;
        }
    }
}