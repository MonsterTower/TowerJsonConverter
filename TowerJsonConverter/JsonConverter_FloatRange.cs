using System;
using Newtonsoft.Json;

namespace TowerJsonConverter
{
    /// <summary>
    /// FloatRange
    /// 1.二元写法[x,y]
    /// 2.一元写法x，此时x=y
    /// </summary>
    public class JsonConverter_FloatRange : JsonConverter<FloatRange>
    {
        public override void WriteJson(JsonWriter writer, FloatRange value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.min);
            writer.WriteValue(value.max);
            writer.WriteEndArray();
        }
        public override FloatRange ReadJson(JsonReader reader, Type objectType, FloatRange existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read(); // min
                float min = Convert.ToSingle(reader.Value);
                reader.Read(); // max
                float max = Convert.ToSingle(reader.Value);
                reader.Read(); // EndArray
                return new FloatRange(min, max);
            }
            else if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
            {
                float value = Convert.ToSingle(reader.Value);
                return new FloatRange(value, value);
            }

            throw new JsonSerializationException("非法的FloatRange格式.");
        }
    }
    /// <summary>
    /// 临时的FloatRange，使用时自行替换
    /// </summary>
    public struct FloatRange
    {
        public float min;
        public float max;
        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}