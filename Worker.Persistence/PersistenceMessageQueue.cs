using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Worker.Implementation;
using Worker.Interface;

namespace Worker.Persistence
{
    public static class RepositoryProvider
    {
        private static Repository Repository { get; set; }

        public static Repository GetRepository()
        {
            if (Repository == null)
            {
                Repository=new Repository("D:\\temp");
            }
            return Repository;
        }
    }
    public class Repository
    {
        private readonly string _dir;

        public Repository(string dir)
        {
            _dir = dir;
        }
        public void Save<T>(string type, T data)
        {
            var path = $"{_dir}\\{type}_{typeof(T).Name}";
            var stringValue = JsonConvert.SerializeObject(data,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented });
            File.WriteAllText(path, stringValue);
        }

        public T Get<T>(string type)
        {
            var path = $"{_dir}\\{type}_{typeof(T).Name}";
            var jsonSrt =File.Exists(path)?File.ReadAllText(path):"";
            return JsonConvert.DeserializeObject<T>(jsonSrt,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
    public class PersistenceMessageQueue : DefaultMessageQueue
    {
        private readonly ILogger _logger;

        public PersistenceMessageQueue(ILogger logger) : base(logger)
        {
            _logger = logger;
        }

        public override Queue<Messgae> Load()
        {
            return RepositoryProvider.GetRepository().Get<Queue<Messgae>>("MessgaeQueue") ?? new Queue<Messgae>();
        }

        public override void Save()
        {
            RepositoryProvider.GetRepository().Save<Queue<Messgae>>("MessgaeQueue",Queue);
        }
    }

    public class PersistenceErrorMessageManager : IErrorMessageManager
    {
        private List<Messgae> ErrorMessage { get; set; }
        public PersistenceErrorMessageManager()
        {
            ErrorMessage = RepositoryProvider.GetRepository().Get<List<Messgae>>("ErrorMessage")??new List<Messgae>();
        }
        public List<Messgae> GetAll()
        {
            return ErrorMessage;
        }

        public void AddErrorMessgae(Messgae messgae)
        {
            ErrorMessage.Add(messgae);
        }

        public void Save()
        {
            RepositoryProvider.GetRepository().Save<List<Messgae>>("ErrorMessage", ErrorMessage);
        }
    }
    public class PersistenceHistoryMessageManager : IHistoryMessageManager
    {
        private List<Messgae> HistoryMessage { get; set; }

        public PersistenceHistoryMessageManager()
        {
            HistoryMessage = RepositoryProvider.GetRepository().Get<List<Messgae>>("HistoryMessage") ?? new List<Messgae>(); 
        }
        public List<Messgae> GetAll()
        {
            return HistoryMessage;
        }

        public void AddHistoryMessgae(Messgae messgae)
        {
            HistoryMessage.Add(messgae);
        }

        public void Save()
        {
            RepositoryProvider.GetRepository().Save<List<Messgae>>("HistoryMessage", HistoryMessage);
        }
    }
}