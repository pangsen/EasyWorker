using System;
using System.Collections.Generic;
using System.Linq;
using Worker.Implementation;
using Worker.Interface;

namespace Worker.Persistence
{
    public interface IPersistenceMessageManager : IMessageManager
    {
        void Save();
        void Load();
    }
    public class PersistenceMessageManager : IPersistenceMessageManager
    {
        private readonly ILogger _logger;
        private readonly string _persistIdentity;
        private static object _lock = new object();
        private List<Message> HistoryMessage { get; set; }
        private List<Message> ErrorMessage { get; set; }
        private List<Message> PendingMessage { get; set; }
        public IQueueMessager QueueMessager { get; set; }
        public PersistenceMessageManager(ILogger logger, string persistIdentity)
        {
            _logger = logger;
            _persistIdentity = persistIdentity;
            QueueMessager = new DefaultQueueMessager(logger);
        }
        public List<Message> GetHistoryMessages()
        {

            return HistoryMessage;
        }

        public List<Message> GetErrorMessages()
        {
            return ErrorMessage;
        }

        public List<Message> GetPendingMessages()
        {
            return PendingMessage;
        }

        public void AddHistoryMessgae(Message message)
        {
            lock (_lock)
            {
                PendingMessage.Remove(PendingMessage.Single(a => a.Id == message.Id));
                HistoryMessage.Add(message);
            }

        }

        public void AddErrorMessgae(Message message)
        {
            lock (_lock)
            {
                PendingMessage.Remove(PendingMessage.Single(a => a.Id == message.Id));
                ErrorMessage.Add(message);
            }
        }

        public void AddPendingMessgae(Message message)
        {
            lock (_lock)
            {
                PendingMessage.Add(message);
            }
        }

        public void RemoveErrorMessage(Guid id)
        {
            lock (_lock)
            {
                ErrorMessage.Remove(ErrorMessage.Single(a => a.Id == id));
            }
        }

        public void Save()
        {
            lock (_lock)
            {
                RepositoryProvider.GetRepository().Save<List<Message>>($"{_persistIdentity}_HistoryMessage", HistoryMessage);
                RepositoryProvider.GetRepository().Save<List<Message>>($"{_persistIdentity}_ErrorMessage", ErrorMessage);
                RepositoryProvider.GetRepository().Save<List<Message>>($"{_persistIdentity}_PendingMessage", PendingMessage);
                RepositoryProvider.GetRepository().Save<List<Message>>($"{_persistIdentity}_QueuedMessage", QueueMessager.GetQueuedMessages());
            }
        }

        public void Load()
        {
            HistoryMessage = RepositoryProvider.GetRepository().Get<List<Message>>($"{_persistIdentity}_HistoryMessage") ?? new List<Message>();
            ErrorMessage = RepositoryProvider.GetRepository().Get<List<Message>>($"{_persistIdentity}_ErrorMessage") ?? new List<Message>();
            PendingMessage = RepositoryProvider.GetRepository().Get<List<Message>>($"{_persistIdentity}_PendingMessage") ?? new List<Message>();
            PendingMessage.ForEach(QueueMessager.Enqueue);
            var queuedMessages = RepositoryProvider.GetRepository().Get<List<Message>>($"{_persistIdentity}_QueuedMessage") ?? new List<Message>();
            queuedMessages.ForEach(QueueMessager.Enqueue);
        }
    }
}