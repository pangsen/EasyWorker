using System;
using Worker.Implementation;
using Worker.Interface;

namespace Worker
{
    public class WorkerOption
    {
        public IMessageQueue MessageQueue { get; set; }
        public IConsumer Consumer { get; set; }
        public IHandlerManager HandlerManager { get; set; }
        public static WorkerOption New => new WorkerOption();
        public ILogger Logger { get; set; }
        private int MaxTaskCount { get; set; }
        private int DelaySecondsWhenNoMessageCome { get; set; }
        public Func<WorkerOption, IWorker> WorkerFactory { get; set; }

        public WorkerOption SetMaxTaskCount(int maxTaskCount)
        {
            MaxTaskCount = maxTaskCount;
            return this;
        }

        public WorkerOption SetDelaySecondsWhenNoMessageCome(int delaySecondsWhenNoMessageCome)
        {
            DelaySecondsWhenNoMessageCome = delaySecondsWhenNoMessageCome;
            return this;
        }
        public IWorker CreateWorker()
        {
            Logger = new ConsoleLogger();
            MessageQueue = new DefaultMessageQueue(Logger);
            HandlerManager = new DefaultHandlerManager(Logger);
            Consumer = new MultipleThreadConsumer(MessageQueue, HandlerManager, Logger);
            if (MaxTaskCount != 0)
            {
                Consumer.SetMaxTaskCount(MaxTaskCount);

            }
            if (DelaySecondsWhenNoMessageCome != 0)
            {
                Consumer.SetDelaySecondsWhenNoMessageCome(MaxTaskCount);
            }
            if (WorkerFactory != null)
            {
                return WorkerFactory(this);
            }
            return new Worker(this);
        }
    }
}