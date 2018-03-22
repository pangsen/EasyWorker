using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Interface
{
    public interface IHistoryMessageManager
    {
        List<Messgae> GetAll();
        void AddHistoryMessgae(Messgae messgae);
        void Save();
    }
}