using System;
using System.Linq;
using Newtonsoft.Json;

namespace TowerJsonConverter
{
     /// <summary>
    /// Type
    /// 1.完整写法"名字空间.类名,程序集名"
    /// 2.本程序集写法"名字空间.类名"，缺省的程序集名填充为Assembly-CSharp
    /// 3.名字空间缺省写法"类名,程序集名"，会从目标程序集里面找到第一个同类名的
    /// 4.最简写法"类名"，会从Assembly-CSharp里面找到第一个同类名的
    /// </summary>
    public class JsonConverter_Type : JsonConverter<Type>
    {

        public override void WriteJson(JsonWriter writer, Type value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            // 获取类型的程序集名
            string assemblyName = value.Assembly.GetName().Name;

            // 如果程序集名是默认程序集名，则只写类型名
            if (assemblyName == Main.DefaultAssemblyName)
            {
                writer.WriteValue(value.Name); // 只写类型名
            }
            else
            {
                // 否则，写完整的类型名，包含程序集名
                writer.WriteValue($"{value.FullName}, {assemblyName}");
            }
        }

        public override Type ReadJson(JsonReader reader, Type objectType, Type existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
            {
                string typeName = (string)reader.Value;

                if (string.IsNullOrEmpty(typeName))
                    return null;

                // 尝试根据类型名称查找类型，省略程序集名时默认查找当前程序集
                return BindToType(typeName);
            }

            throw new JsonSerializationException("无法识别的Type格式");
        }

        // 通过类型名称查找类型，省略程序集名时默认为当前程序集
        private Type BindToType(string typeName)
        {
            // 如果有程序集名，则直接使用 Type.GetType
            if (typeName.Contains(","))
            {
                // 处理：如果包含逗号，则说明类型名是带有程序集名的
                var parts = typeName.Split(',');

                if (parts.Length == 2)
                {
                    var assemblyName = parts[1].Trim();
                    var simpleTypeName = parts[0].Trim();

                    // 如果没有命名空间，则默认在该程序集下查找
                    return BindToType(assemblyName, simpleTypeName);
                }
            }

            // 如果没有逗号，且是简单的类型名，默认使用当前程序集（Assembly-CSharp）查找
            return BindToType(Main.DefaultAssemblyName, typeName);
        }

        // 根据类型名和程序集名查找 Type
        private Type BindToType(string assemblyName, string typeName)
        {
            // 如果类型名没有命名空间，尝试查找匹配的类型
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName);

            if (assembly != null)
            {
                // 查找匹配类型（支持不带命名空间的情况）
                var type = assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
                if (type != null) return type;
            }

            throw new JsonSerializationException($"找不到类型: {assemblyName}.{typeName}");
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }
}