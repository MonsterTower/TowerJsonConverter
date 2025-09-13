using System;
using Newtonsoft.Json;

namespace TowerJsonConverter
{
    /// <summary>
    /// Vector2
    /// 1.二元写法[x,y]
    /// 2.一元写法x，此时x=y
    /// </summary>
    public class JsonConverter_Vector2 : JsonConverter<Vector2>
    {
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            // 序列化成 [x, y] 格式
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteEndArray();
        }
        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                // 处理 [x, y] 格式
                reader.Read(); // 读取开始数组
                float x = Convert.ToSingle(reader.Value); // 读取 x
                reader.Read(); // 读取逗号
                float y = Convert.ToSingle(reader.Value); // 读取 y
                reader.Read(); // 读取结束数组
                return new Vector2(x, y);
            }
            else if (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float)
            {
                // 处理单个数字（1）作为 [1, 1]
                float value = Convert.ToSingle(reader.Value);
                return new Vector2(value, value); // 返回 Vector2(value, value)
            }

            throw new JsonSerializationException("非法格式Vector2.");
        }
    }
    public struct Vector2
    {
        public float x;
        public float y;
        public Vector2(float x,float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}