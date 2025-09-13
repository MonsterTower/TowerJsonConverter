using System;
using Newtonsoft.Json;

namespace TowerJsonConverter
{
    /// <summary>
    /// IntRange
    /// 1.二元写法[x,y]
    /// 2.一元写法x，此时x=y
    /// </summary>
    public class JsonConverter_IntRange : JsonConverter<IntRange>
    {
        public override void WriteJson(JsonWriter writer, IntRange value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.min);
            writer.WriteValue(value.max);
            writer.WriteEndArray();
        }

        public override IntRange ReadJson(JsonReader reader, Type objectType, IntRange existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read(); // min
                int min = Convert.ToInt32(reader.Value);
                reader.Read(); // max
                int max = Convert.ToInt32(reader.Value);
                reader.Read(); // EndArray
                return new IntRange(min, max);
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                int value = Convert.ToInt32(reader.Value);
                return new IntRange(value, value);
            }

            throw new JsonSerializationException("Invalid format for IntRange.");
        }
    }
    public struct IntRange
    {
        public int min;
        public int max;
        public IntRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
}