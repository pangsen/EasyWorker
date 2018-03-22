using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Persistence
{
    public class PersistenceWorker : Worker, IPersistenceWorker
    {
        public PersistenceWorker(WorkerOption options) : base(options) { }

    

        public void Save()
        {
            MessageQueue.Save();
            MessageManager.Save();
            MessageManager.Save();
        }
    }
}
