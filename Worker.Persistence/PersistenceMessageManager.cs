using System;
using System.Collections.Generic;
using System.Linq;
using Worker.Implementation;
using Worker.Interface;

namespace Worker.Persistence
{
    public class PersistenceMessageManager : IMessageManager
    {
        private static object _lock = new object();
        private List<Message> HistoryMessage { get; set; }
        private List<Message> ErrorMessage { get; set; }
        private List<Message> PendingMessage { get; set; }

        public PersistenceMessageManager()
        {
            HistoryMessage = RepositoryProvider.GetRepository().Get<List<Message>>("HistoryMessage") ?? new List<Message>();
            ErrorMessage = RepositoryProvider.GetRepository().Get<List<Message>>("ErrorMessage") ?? new List<Message>();
            PendingMessage = RepositoryProvider.GetRepository().Get<List<Message>>("PendingMessage") ?? new List<Message>();
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
                RepositoryProvider.GetRepository().Save<List<Message>>("HistoryMessage", HistoryMessage);
                RepositoryProvider.GetRepository().Save<List<Message>>("ErrorMessage", ErrorMessage);
                RepositoryProvider.GetRepository().Save<List<Message>>("PendingMessage", PendingMessage);
            }
        }
    }
}