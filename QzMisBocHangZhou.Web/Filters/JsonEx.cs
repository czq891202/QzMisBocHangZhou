using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace QzMisBocHangZhou.Web.Filters
{
    /// <summary>
    /// Json操作类
    /// </summary>
    public static class JsonEx
    {
        /// <summary>
        /// Json格式: 默认为驼峰格式
        /// </summary>
        private static readonly JsonSerializerSettings s_JsonSetting = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        /// <summary>
        /// 对象 to Json字符串 (CamelCase格式)
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="obj">待转换为Json的对象</param>
        /// <returns>对象Json序列化后生成的Json字符串</returns>
        public static string ToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, s_JsonSetting);
        }


        /// <summary>
        /// Json字符串 To 对象 (CamelCase格式)
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T FromJson<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr, s_JsonSetting);
        }
    }
}