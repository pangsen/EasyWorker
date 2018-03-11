namespace Worker
{
    public interface IConsumer
    {
        int PendingTaskCount { get; set; }
        IMessageQueue MessageQueue { get; set; }
        IHandlerManager HandlerManager { get; set; }
        void Stop();
        void Start();
    }
}