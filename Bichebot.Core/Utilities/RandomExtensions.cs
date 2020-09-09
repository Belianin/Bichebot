using System;
using System.Collections.Generic;
using System.Linq;

namespace Bichebot.Core.Utilities
{
    public static class RandomExtensions
    {
        public static T Choose<T>(this Random random, params T[] source)
        {
            var array = source.ToArray();
            return array[random.Next(0, array.Length)];
        } 
        
        public static T Choose<T>(this Random random, IList<T> source)
        {
            return source[random.Next(0, source.Count)];
        }
        
        public static T Choose<T>(this Random random, ICollection<T> source)
        {
            var array = source.ToArray();
            return array[random.Next(0, array.Length)];
        }

        public static T Choose<T>(this Random random, IReadOnlyCollection<T> source)
        {
            var array = source.ToArray();
            return array[random.Next(0, array.Length)];
        }
        
        public static T Choose<T>(this Random random, IEnumerable<T> source)
        {
            var array = source.ToArray();
            return array[random.Next(0, array.Length)];
        }
        
        public static T Choose<T>(this Random random, IEnumerable<T> source, Func<T, double> weight)
        {
            var rates = new Dictionary<double, T>();
            var sum = 0d;
            foreach (var c in source)
            {
                sum += weight(c);
                rates[sum] = c;
            }
            
            var rate = random.NextDouble() * sum;
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