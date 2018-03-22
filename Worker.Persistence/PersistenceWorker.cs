using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Persistence
{
    public class PersistenceWorker : Worker, IPersistenceWorker
    {
        public PersistenceWorker(WorkerOption options) : base(options) { }

        public List<Messgae> GetErrorMessages()
        {
            return ErrorMessageManager.GetAll();
        }

        public List<Messgae> GetHistoryMessages()
        {
            return HistoryMessageManager.GetAll();
        }

        public void Save()
        {
            MessageQueue.Save();
            ErrorMessageManager.Save();
            HistoryMessageManager.Save();
        }
    }
}
