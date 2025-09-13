using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TowerJsonConverter
{
    /// <summary>
    /// 对Json中的$type进行修改
    /// 完整规范："$type": "Night.Core.Data.EntityDef, Assembly-CSharp"
    /// 使用该DefTypeBinder后：
    /// 可以省略程序集，默认从当前程序集(Assembly-CSharp)里面找
    /// 可以缺省名字空间，会自动从目标程序集里找到第一个匹配的Def类型
    /// 最终用法:"$type":"EntityDef"
    /// </summary>
    public class TypeBinder : ISerializationBinder
    {

        public Type BindToType(string assemblyName, string typeName)
        {
            // 确定要查找的程序集名，缺省用默认
            string asmName = string.IsNullOrEmpty(assemblyName) ? Main.DefaultAssemblyName : assemblyName;

            // 先尝试用完整名称 + 程序集名直接查找（带命名空间或不带均可）
            string fullName = typeName.Contains(".") ? $"{typeName}, {asmName}" : null;
            if (fullName != null)
            {
                var type = Type.GetType(fullName);
                if (type != null) return type;
            }

            // 如果没找到或者 typeName 无命名空间，尝试在程序集内遍历类型查找
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == asmName);
            if (assembly != null)
            {
                var type = assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
                if (type != null) return type;
            }

            throw new JsonSerializationException($"找不到类型: {asmName}.{typeName}");
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.GetName().Name;
            typeName = serializedType.FullName;
        }
    }

}