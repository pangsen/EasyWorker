using System;
using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Persistence
{
    public class PersistenceWorker : Worker, IPersistenceWorker
    {
        public PersistenceWorker(WorkerOption options) : base(options)
        {
            LoadDataForMessageManager();
        }
        public void SaveState()
        {
            if (!(MessageManager is IPersistenceMessageManager persistenceMessageManager))
            {
                throw new Exception("Current message manager not implement IPersistenceMessageManager");
            }
            persistenceMessageManager.Save();
        }

        public void LoadDataForMessageManager()
        {
            if (!(MessageManager is IPersistenceMessageManager persistenceMessageManager))
            {
                throw new Exception("Current message manager can not persist");
            }
            persistenceMessageManager.Load();
        }
    }
}
