using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace QueueManager.QueueManagement
{
    public static class ByteArraySerializer
    {
        public static byte[] Serialize<T>(this T m)
        {
            var text = JsonSerializer.Serialize(m);
            return Encoding.UTF8.GetBytes(text);
        }

        public static T Deserialize<T>(this IEnumerable<byte> byteArray)
        {
            var bytes = Encoding.UTF8.GetString(byteArray.ToArray());
            return JsonSerializer.Deserialize<T>(bytes);
        }
    }
}