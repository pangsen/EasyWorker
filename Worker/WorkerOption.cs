using System;
using Worker.Implementation;
using Worker.Interface;

namespace Worker
{
    public class WorkerOption
    {

        public Func<WorkerOption, IMessageQueue> MessageQueueFatory { get; set; }
        public Func<WorkerOption, IConsumer> ConsumerFatory { get; set; }
        public Func<WorkerOption, IHandlerManager> HandlerManagerFatory { get; set; }
        public Func<WorkerOption, IMessageManager> MessageManagerFatory { get; set; }
        public Func<WorkerOption, ILogger> LoggerFatory { get; set; }
        public static WorkerOption New => new WorkerOption();
        private int MaxTaskCount { get; set; }
        private int DelaySecondsWhenNoMessageCome { get; set; }
        public IMessageQueue MessageQueue { get; private set; }
        public IConsumer Consumer { get; private set; }
        public IHandlerManager HandlerManager { get; private set; }
        public IMessageManager MessageManager { get; private set; }
        public ILogger Logger { get; private set; }

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
            Logger = LoggerFatory != null ? LoggerFatory(this) : new ConsoleLogger();
            MessageQueue = MessageQueueFatory != null ? MessageQueueFatory(this) : new DefaultMessageQueue(Logger);
            HandlerManager = HandlerManagerFatory != null ? HandlerManagerFatory(this) : new DefaultHandlerManager(Logger);
            MessageManager = MessageManagerFatory != null ? MessageManagerFatory(this) : new DefaultMessageManager(Logger);
            Consumer = ConsumerFatory != null ? ConsumerFatory(this) : new MultipleThreadConsumer(MessageQueue, HandlerManager, Logger, MessageManager);
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