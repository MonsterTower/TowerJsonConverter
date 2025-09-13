using System;
using Newtonsoft.Json;

namespace TowerJsonConverter
{
    /// <summary>
    /// Vector3
    /// 1.三元写法[x,y,z]
    /// 2.二元写法[x,y]，那么z默认为0
    /// 3.一元写法x,那么x=y，z为0
    /// </summary>
    public class JsonConverter_Vector3 : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            // 将 Vector3 写为 [x, y, z] 格式
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteValue(value.z);
            writer.WriteEndArray();
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                // 处理 [x, y, z] 格式
                reader.Read(); // x
                float x = Convert.ToSingle(reader.Value);
                reader.Read(); // y
                float y = Convert.ToSingle(reader.Value);
                float z = 0;

                if (reader.TokenType != JsonToken.EndArray)
                {
                    reader.Read(); // z
                    z = Convert.ToSingle(reader.Value);
                }
                reader.Read(); // EndArray

                return new Vector3(x, y, z);
            }
            else if (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float)
            {
                // 处理单个数字作为 x = y = value
                float value = Convert.ToSingle(reader.Value);
                return new Vector3(value, value, 0);
            }

            throw new JsonSerializationException("非法的Vector3格式.");
        }
    }
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float x,float y,float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}