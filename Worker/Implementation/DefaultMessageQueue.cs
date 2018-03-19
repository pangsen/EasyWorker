using System.Collections.Generic;
using System.Linq;
using Worker.Interface;

namespace Worker.Implementation
{
    public class DefaultMessageQueue : IMessageQueue
    {
        private readonly ILogger _logger;
        private readonly object _lock = new object();

        private Queue<IMessgae> Queue { get; set; }


        public DefaultMessageQueue(ILogger logger)
        {
            _logger = logger;
            Queue = new Queue<IMessgae>();
        }
        public void Enqueue(IMessgae message)
        {
            lock (_lock)
            {
                Queue.Enqueue(message);
            }

        }

        public IMessgae Dequeue()
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
    }
}