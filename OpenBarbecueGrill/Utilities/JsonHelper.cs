using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenBarbecueGrill.Utilities
{
    public class JsonHelper
    {
        public static JsonSerializerOptions ConfigurationOptions { get; } =
            new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,

                Converters =
                {
                    new JsonStringEnumConverter(),
                }
            };
    }
}
