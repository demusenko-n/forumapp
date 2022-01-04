using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MIST_app.Services
{   
    public class JsonService
    {
        private JsonService()
        {
            JsonSerializerOptions = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PropertyNameCaseInsensitive = true
            };
        }
        public static JsonService Instance { get; } = new JsonService();
        public JsonSerializerOptions JsonSerializerOptions { get; }
        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, options: JsonSerializerOptions);
        }

        public string Serialize(object obj, Type type)
        {
            return JsonSerializer.Serialize(obj, type, options: JsonSerializerOptions);
        }
        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, options: JsonSerializerOptions);
        }
    }
}
