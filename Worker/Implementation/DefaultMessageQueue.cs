using System.Collections.Generic;
using System.Linq;
using Worker.Interface;

namespace Worker.Implementation
{
    public class DefaultMessageQueue : IMessageQueue
    {
        private readonly ILogger _logger;
        private readonly object _lock = new object();
        protected Queue<Messgae> Queue { get; set; }
        public DefaultMessageQueue(ILogger logger)
        {
            _logger = logger;
            Queue = Load();
        }
        public void Enqueue(Messgae message)
        {
            lock (_lock)
            {
                Queue.Enqueue(message);
            }

        }

        public Messgae Dequeue()
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

        public virtual void Save()
        {
            throw new System.NotImplementedException();
        }

        public virtual Queue<Messgae> Load()
        {
            return new Queue<Messgae>();
        }
    }
}