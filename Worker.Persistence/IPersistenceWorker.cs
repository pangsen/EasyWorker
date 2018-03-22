using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Persistence
{
    public interface IPersistenceWorker : IWorker
    {
        List<Messgae> GetErrorMessages();
        List<Messgae> GetHistoryMessages();
        void Save();
    }
}