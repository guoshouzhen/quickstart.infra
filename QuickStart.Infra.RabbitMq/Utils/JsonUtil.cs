using Newtonsoft.Json;

namespace QuickStart.Infra.RabbitMq.Utils
{
    public class JsonUtil
    {
        /// <summary>
        /// Serialize the object to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Object2Json<T>(T data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            try
            {
                return JsonConvert.SerializeObject(data);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deserialize json to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T? Json2Object<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                throw;
            }
        }
    }
}
