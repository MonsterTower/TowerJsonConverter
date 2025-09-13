using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TowerJsonConverter
{
    /// <summary>
    /// Color
    /// 1. 16进制读法，要求输入是#RRGGBBAA，自动转化成Color
    /// 2. 16进制读法，要求输入是#RRGGBB，自动转化成Color，alpha保持1f
    /// 3. 三数读法[r,g,b]，输入是0~255，自动转换成0~1f然后做出Color，alpha保持1f
    /// 4. 四数读法[r,g,b,a]，输入是0~255，自动转换成0~1f然后做出Color，
    /// </summary>
    public class JsonConverter_Color : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            // 序列化成 #RRGGBBAA 格式
            string hex = $"#{(byte)(value.r * 255):X2}{(byte)(value.g * 255):X2}{(byte)(value.b * 255):X2}{(byte)(value.a * 255):X2}";
            writer.WriteValue(hex);
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                // 处理字符串类型，比如 #RRGGBB 或 #RRGGBBAA
                string s = (string)reader.Value;
                return ParseHexColor(s);
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                // 处理数组类型，比如 [r,g,b] 或 [r,g,b,a]
                JArray array = JArray.Load(reader);
                return ParseArrayColor(array);
            }

            throw new JsonSerializationException("无法识别的Color格式");
        }

        private Color ParseHexColor(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return new Color(1f, 1f, 1f, 1f);

            hex = hex.Trim();

            if (!hex.StartsWith("#"))
                throw new JsonSerializationException("Hex颜色必须以#开头");

            // 去掉 #
            hex = hex.Substring(1);

            byte r, g, b, a = 255;

            try
            {
                if (hex.Length == 6) // RRGGBB
                {
                    r = Convert.ToByte(hex.Substring(0, 2), 16);
                    g = Convert.ToByte(hex.Substring(2, 2), 16);
                    b = Convert.ToByte(hex.Substring(4, 2), 16);
                }
                else if (hex.Length == 8) // RRGGBBAA
                {
                    r = Convert.ToByte(hex.Substring(0, 2), 16);
                    g = Convert.ToByte(hex.Substring(2, 2), 16);
                    b = Convert.ToByte(hex.Substring(4, 2), 16);
                    a = Convert.ToByte(hex.Substring(6, 2), 16);
                }
                else
                {
                    throw new JsonSerializationException("无效的Hex颜色长度，应为#RRGGBB或#RRGGBBAA");
                }
            }
            catch
            {
                throw new JsonSerializationException("无效的Hex颜色格式，无法解析十六进制数值");
            }

            // 将 byte 转换成 float (0~1)
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }


        private Color ParseArrayColor(JArray array)
        {
            if (array.Count == 3 || array.Count == 4)
            {
                float r = Convert.ToSingle(array[0]) / 255f;
                float g = Convert.ToSingle(array[1]) / 255f;
                float b = Convert.ToSingle(array[2]) / 255f;
                float a = 1f;

                if (array.Count == 4)
                {
                    a = Convert.ToSingle(array[3]) / 255f;
                }

                return new Color(r, g, b, a);
            }

            throw new JsonSerializationException("颜色数组格式应为[r,g,b]或[r,g,b,a]");
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }
    public struct Color
    {
        public float r;
        public float g;
        public float b;
        public float a;
        public Color(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1f;
        }
    }
}