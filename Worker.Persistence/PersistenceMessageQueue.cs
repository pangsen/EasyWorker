using System.Collections.Generic;
using Worker.Implementation;
using Worker.Interface;

namespace Worker.Persistence
{
    public class PersistenceMessageQueue : DefaultMessageQueue
    {
        private readonly ILogger _logger;

        public PersistenceMessageQueue(ILogger logger) : base(logger)
        {
            _logger = logger;
        }

        public override Queue<Message> Load()
        {
            return RepositoryProvider.GetRepository().Get<Queue<Message>>("MessgaeQueue") ?? new Queue<Message>();
        }

        public override void Save()
        {
            RepositoryProvider.GetRepository().Save<Queue<Message>>("MessgaeQueue",Queue);
        }
    }
}