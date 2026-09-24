using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameEngine
{
    /// <summary>
    /// Converts the engine's <see cref="Stack{T}"/>-based card collection while preserving stack order.
    /// </summary>
    public class StackConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this converter handles the engine's card stack type.
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Stack<ICard>);
        }

        /// <summary>
        /// Reads a card stack from JSON without reversing the stack order.
        /// </summary>
        /// <param name="reader">The JSON reader.</param>
        /// <param name="objectType">The target object type.</param>
        /// <param name="existingValue">An existing stack instance, when available.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>The deserialized card stack.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null!;
            }

            var cards = serializer.Deserialize<List<ICard>>(reader)
                ?? throw new JsonSerializationException("The card stack JSON value could not be deserialized.");

            var stack = existingValue as Stack<ICard> ?? new Stack<ICard>();

            // Json.NET reads the serialized stack from top to bottom. Push in reverse
            // order so Stack.Pop() returns the same card that was originally on top.
            for (var i = cards.Count - 1; i >= 0; i--)
            {
                stack.Push(cards[i]);
            }

            return stack;
        }

        /// <summary>
        /// Writes a card stack as an array in its enumeration order.
        /// </summary>
        /// <param name="writer">The JSON writer.</param>
        /// <param name="value">The card stack to serialize.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not Stack<ICard> stack)
            {
                throw new JsonSerializationException("StackConverter can only serialize Stack<ICard> values.");
            }

            writer.WriteStartArray();
            foreach (var card in stack)
            {
                serializer.Serialize(writer, card, typeof(ICard));
            }
            writer.WriteEndArray();
        }
    }
}
