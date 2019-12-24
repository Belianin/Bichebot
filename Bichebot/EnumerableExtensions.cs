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
        
        public static T Random<T>(this IEnumerable<T> collection, Random rnd, Func<T, int> weight = null)
        {
            if (weight == null)
                return collection.ToList().Random(rnd);
            
            var rates = new Dictionary<int, T>();
            var sum = 0;
            foreach (var c in collection)
            {
                rates[sum] = c;
                sum += weight(c);
            }
            
            var rate = rnd.Next(0, sum);
            return rates.OrderByDescending(r => r.Key).First(k => k.Key < rate).Value;
        }
    }
}