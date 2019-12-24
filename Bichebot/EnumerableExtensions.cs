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
    }
}