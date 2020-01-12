using System;
using System.Collections.Generic;
using System.Linq;

namespace Bichebot.Utilities
{
    public static class RandomExtensions
    {
        public static T Choose<T>(this Random random, IEnumerable<T> source)
        {
            var array = source.ToArray();
            return array[random.Next(0, array.Length)];
        }
        
        public static T Choose<T>(this Random random, IEnumerable<T> source, Func<T, int> weight)
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

        public static bool Roll(this Random random, int numerator, int denominator)
        {
            return random.Next(denominator) < numerator;
        }
        
        public static bool Roll(this Random random, int chance)
        {
            return random.Roll(1, chance);
        }
    }
}