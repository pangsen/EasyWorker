namespace Worker
{
    public class WorkerOption
    {
        public IMessageQueue MessageQueue { get; set; }
        public IConsumer Consumer { get; set; }
        public IHandlerManager HandlerManager { get; set; }
        public static WorkerOption New => new WorkerOption();

        private WorkerOption()
        {
            MessageQueue = new DefaultMessageQueue();
            HandlerManager = new DefaultHandlerManager();
            Consumer = new MultipleThreadConsumer(MessageQueue, HandlerManager);
        }
        public Worker CreateWorker()
        {
            return new Worker(this);
        }

    }
}