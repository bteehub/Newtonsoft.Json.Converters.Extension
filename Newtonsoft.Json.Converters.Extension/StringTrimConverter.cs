using System;
using System.Runtime.CompilerServices;

#if !HAVE_FULL_REFLECTION
using System.Reflection;
#endif

[assembly: InternalsVisibleTo("Newtonsoft.Json.Converters.Extension.Test")]
namespace Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="string"/> with trimming
    /// </summary>
    public class StringTrimConverter : JsonConverter<string>
    {
        public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
        {
            writer.WriteValue(value?.Trim());
        }

        public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var result = (reader?.Value as string ?? string.Empty).Trim();

            if (hasExistingValue)
                result += existingValue?.Trim() ?? string.Empty;

            return result;
        }
    }
}
