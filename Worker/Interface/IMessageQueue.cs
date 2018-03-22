using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Interface
{
    public interface IMessageQueue
    {
        void Enqueue(Message message);
        Message Dequeue();
        int Count { get; }
        bool HasValue();
        void Save();
        Queue<Message> Load();
        List<Message> GetAll();
    }
}