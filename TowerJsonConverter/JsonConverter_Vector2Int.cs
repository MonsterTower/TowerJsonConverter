using System;
using Newtonsoft.Json;

namespace TowerJsonConverter
{
    /// <summary>
    /// Vector2Int
    /// 1.二元写法[x,y]
    /// 2.一元写法x，此时x=y
    /// </summary>
    public class JsonConverter_Vector2Int : JsonConverter<Vector2Int>
    {
        public override void WriteJson(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
        {
            // 序列化成 [x, y] 格式
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteEndArray();
        }

        public override Vector2Int ReadJson(JsonReader reader, Type objectType, Vector2Int existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                // 处理 [x, y] 格式
                reader.Read(); // 读取开始数组
                int x = Convert.ToInt32(reader.Value); // 读取 x
                reader.Read(); // 读取逗号
                int y = Convert.ToInt32(reader.Value); // 读取 y
                reader.Read(); // 读取结束数组
                return new Vector2Int(x, y);
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                // 处理单个数字（1）作为 [1, 1]
                int value = Convert.ToInt32(reader.Value);
                return new Vector2Int(value, value); // 返回 Vector2Int(value, value)
            }
            throw new JsonSerializationException("无法识别的Vector2Int格式.");
        }
    }
    public struct Vector2Int
    {
        public int x;
        public int y;
        public Vector2Int(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}