using System.Collections.Generic;
using Newtonsoft.Json;

namespace TowerJsonConverter
{
    public static class Main
    {
        static Main()
        {
            Settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.None,
                SerializationBinder = new TypeBinder(),
                MissingMemberHandling = MissingMemberHandling.Error,
                Converters = new List<JsonConverter>
                {
                    new JsonConverter_FloatRange(),
                    new JsonConverter_Color(),
                    new JsonConverter_IntRange(),
                    new JsonConverter_Type(),
                    new JsonConverter_Vector2(),
                    new JsonConverter_Vector2Int(),
                    new JsonConverter_Vector3(),
                    new JsonConverter_Vector3Int()
                    //可以自行往上增加内容
                },
            };
        }
        //根据需要自行修改
        public const string DefaultAssemblyName = "Assembly-CSharp";
        public static readonly JsonSerializerSettings Settings;
    }
}