namespace Worker.Interface
{
    public interface IMessageQueue
    {
        void Enqueue(IMessgae message);
        IMessgae Dequeue();
        int Count { get; }
        bool HasValue();
    }
}