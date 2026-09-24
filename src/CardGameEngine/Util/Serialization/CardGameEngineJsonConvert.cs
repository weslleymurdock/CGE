using System;
using Newtonsoft.Json;

namespace CardGameEngine
{
    public static class CardGameEngineJsonConvert
    {
        public static readonly JsonSerializerSettings serializerSettings = new()

        {
            Converters = new[] { new StackConverter() },
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

/// <summary>Serializes an object to JSON.</summary>
/// <param name="obj">The obj value.</param>
/// <returns>The result of the operation.</returns>
        public static string Serialize(Object obj)
        {
            return JsonConvert.SerializeObject(obj, serializerSettings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, serializerSettings) ?? throw new InvalidOperationException("Deserialization was not possible for this data.");
        }
    }
}
