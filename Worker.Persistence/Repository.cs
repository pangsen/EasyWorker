using System.IO;
using Newtonsoft.Json;

namespace Worker.Persistence
{
    public class Repository
    {
        private readonly string _dir;

        public Repository(string dir)
        {
            _dir = dir;
        }
        public void Save<T>(string id, T data)
        {
            var path = $"{_dir}\\{id}_{typeof(T).Name}";
            var stringValue = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented });
            File.WriteAllText(path, stringValue);
        }

        public T Get<T>(string id)
        {
            var path = $"{_dir}\\{id}_{typeof(T).Name}";
            var jsonSrt = File.Exists(path) ? File.ReadAllText(path) : "";
            return JsonConvert.DeserializeObject<T>(jsonSrt, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}