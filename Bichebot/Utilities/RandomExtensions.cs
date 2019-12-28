using System;
using System.Collections.Generic;
using System.Linq;

namespace Bichebot.Utilities
{
    public static class RandomExtensions
    {
        public static T GetRandom<T>(this Random random, ICollection<T> source)
        {
            return source.ElementAt(random.Next(0, source.Count - 1));
        }
        
        public static T GetRandom<T>(this Random random, IEnumerable<T> source, Func<T, int> weight)
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