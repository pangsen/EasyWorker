using System.Collections.Generic;
using System.Linq;
using Worker.Interface;

namespace Worker.Implementation
{
    public class DefaultQueueMessager : IQueueMessager
    {
        private readonly ILogger _logger;
        private readonly object _lock = new object();
        protected Queue<Message> Queue { get; set; }
        public DefaultQueueMessager(ILogger logger)
        {
            _logger = logger;
            Queue = new Queue<Message>();
        }
        public void Enqueue(Message message)
        {
            lock (_lock)
            {
                Queue.Enqueue(message);
            }

        }

        public Message Dequeue()
        {
            lock (_lock)
            {
                return Queue.Dequeue();
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return Queue.Count;
                }
            }
        }

        public bool HasValue()
        {
            lock (_lock)
            {
                return Queue.Any();
            }
        }

        public List<Message> GetQueuedMessages()
        {
            lock (_lock)
            {
                return Queue.ToList();
            }
        }
    }
}