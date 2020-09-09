using Newtonsoft.Json;

namespace Bichebot.Core.Utilities
{
    public static class StringExtensions
    {
        public static Result<T> TryDeserialize<T>(this string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (JsonException e)
            {
                return Result<T>.Fail(e.Message);
            }
        }
    }
}