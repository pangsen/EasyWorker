using System;
using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Interface
{
    public interface IMessageManager
    {
        List<Message> GetHistoryMessages();
        List<Message> GetErrorMessages();
        List<Message> GetPendingMessages();
        void AddHistoryMessgae(Message message);
        void AddErrorMessgae(Message message);
        void AddPendingMessgae(Message message);
        void RemoveErrorMessage(Guid id);
        void Save();
    }
}