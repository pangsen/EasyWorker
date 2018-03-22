using System;
using System.Collections.Generic;
using System.Linq;
using Worker.Interface;

namespace Worker.Implementation
{
    public class DefaultMessageManager : IMessageManager
    {
        private readonly ILogger _logger;
        private static object _lock = new object();
        private List<Message> Message { get; set; }

        public DefaultMessageManager(ILogger logger)
        {
            _logger = logger;
            Message = new List<Message>();
        }
        public List<Message> GetHistoryMessages()
        {
            lock (_lock)
            {
                return Message.Where(a => a.Status == MessgaeStatus.Completed).ToList();
            }
        }

        public List<Message> GetErrorMessages()
        {
            lock (_lock)
            {
                return Message.Where(a => a.Status == MessgaeStatus.Error).ToList();
            }
        }

        public List<Message> GetPendingMessages()
        {
            lock (_lock)
            {
                return Message.Where(a => a.Status == MessgaeStatus.Pending).ToList();
            }
        }

        public void AddHistoryMessgae(Message message)
        {
            lock (_lock)
            {
                Message.Add(message);
            }
        }

        public void AddErrorMessgae(Message message)
        {
            lock (_lock)
            {
                Message.Add(message);
            }
        }

        public void AddPendingMessgae(Message message)
        {
            lock (_lock)
            {
                Message.Add(message);
            }
        }

        public void RemoveErrorMessage(Guid id)
        {
            lock (_lock)
            {
                Message.Remove(Message.Single(a => a.Id == id));
            }
        }

        public void Save()
        {
            _logger.Write("SaveHistoryMessgae");
        }
    }
}