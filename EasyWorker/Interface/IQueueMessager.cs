using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Interface
{
    public interface IQueueMessager
    {
        void Enqueue(Message message);
        Message Dequeue();
        int Count { get; }
        bool HasValue();
        List<Message> GetQueuedMessages();
    }
}