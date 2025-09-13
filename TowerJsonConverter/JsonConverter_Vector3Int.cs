using System;
using Newtonsoft.Json;

namespace TowerJsonConverter
{
    /// <summary>
    /// Vector3Int
    /// 1.三元写法[x,y,z]
    /// 2.二元写法[x,y]，那么z默认为0
    /// 3.一元写法x,那么x=y，z为0
    /// </summary>
    public class JsonConverter_Vector3Int : JsonConverter<Vector3Int>
    {
        public override void WriteJson(JsonWriter writer, Vector3Int value, JsonSerializer serializer)
        {
            // 将 Vector3Int 写为 [x, y, z] 格式
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteValue(value.z);
            writer.WriteEndArray();
        }

        public override Vector3Int ReadJson(JsonReader reader, Type objectType, Vector3Int existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                // 处理 [x, y, z] 格式
                reader.Read(); // x
                int x = Convert.ToInt32(reader.Value);
                reader.Read(); // y
                int y = Convert.ToInt32(reader.Value);
                int z = 0;

                if (reader.TokenType != JsonToken.EndArray) // [x, y] 格式
                {
                    reader.Read(); // z
                    z = Convert.ToInt32(reader.Value);
                }
                reader.Read(); // EndArray

                return new Vector3Int(x, y, z);
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                // 处理单个数字作为 x = y = value
                int value = Convert.ToInt32(reader.Value);
                return new Vector3Int(value, value, 0);
            }

            throw new JsonSerializationException("非法的Vector3Int格式.");
        }
    }
    public struct Vector3Int
    {
        public int x;
        public int y;
        public int z;
        public Vector3Int(int x,int y,int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}