using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Persistence
{
    public interface IPersistenceWorker : IWorker
    {
        void Save();
    }
}