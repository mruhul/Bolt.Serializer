using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bolt.Serializer.Json
{
    public class JsonSerializer : Bolt.Serializer.ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializer(bool useUtcDate = true, bool useStringEnum = true)
        {
            _settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            if(useUtcDate) _settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            if(useStringEnum) _settings.Converters.Add(new StringEnumConverter());
        }

        public JsonSerializer(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public string Serialize<T>(T value)
        {
            return value == null 
                ? null 
                : JsonConvert.SerializeObject(value, _settings);
        }

        public T Deserialize<T>(string value)
        {
            return string.IsNullOrWhiteSpace(value) 
                    ? default(T) 
                    : JsonConvert.DeserializeObject<T>(value, _settings);
        }

        public T Deserialize<T>(Stream value)
        {
            using (var sr = new StreamReader(value))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    var serializer = new Newtonsoft.Json.JsonSerializer();
                    return serializer.Deserialize<T>(reader);
                }
            }
        }

        public bool IsSupported(string contentType)
        {
            return string.Equals(contentType, ContentTypes.Json);
        }
    }
}
