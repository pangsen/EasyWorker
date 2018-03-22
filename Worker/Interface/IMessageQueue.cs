using System.Collections.Generic;
using Worker.Implementation;

namespace Worker.Interface
{
    public interface IMessageQueue
    {
        void Enqueue(Messgae message);
        Messgae Dequeue();
        int Count { get; }
        bool HasValue();
        void Save();
        Queue<Messgae> Load();
    }
}