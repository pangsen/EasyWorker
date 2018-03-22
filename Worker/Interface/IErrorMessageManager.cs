using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Interface
{
    public interface IErrorMessageManager
    {
        List<Messgae> GetAll();
        void AddErrorMessgae(Messgae messgae);

        void Save();
    }
}