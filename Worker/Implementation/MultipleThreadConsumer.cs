using System;
using System.Threading.Tasks;

namespace Worker
{
    public class MultipleThreadConsumer : IConsumer
    {
        public IMessageQueue MessageQueue { get; set; }
        public IHandlerManager HandlerManager { get; set; }
        private int MaxTaskCount { get; }
        public int PendingTaskCount { get; set; }
        private bool IsStop { get; set; }
        private TimeSpan DelayTimeWhenNoMessageCome { get; }
        public MultipleThreadConsumer(IMessageQueue messageQueue, IHandlerManager handlerManager, int maxTaskCount = 10, int delaySecondsWhenNoMessageCome = 5)
        {
            MessageQueue = messageQueue;
            HandlerManager = handlerManager;
            MaxTaskCount = maxTaskCount;
            DelayTimeWhenNoMessageCome = TimeSpan.FromSeconds(delaySecondsWhenNoMessageCome);
        }
        public void Stop()
        {
            IsStop = true;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (!IsStop)
                {
                    Run();
                }

            });
        }

        private void Run()
        {
            if (PendingTaskCount < MaxTaskCount && MessageQueue.HasValue())
            {
                PendingTaskCount++;
                var message = (dynamic)MessageQueue.Dequeue();
                Task.WhenAny(Task.Run(() =>
                {
                    HandlerManager.Handle(message);
                    PendingTaskCount--;
                })).ContinueWith((t) =>
                {
                    PendingTaskCount--;
                },TaskContinuationOptions.OnlyOnFaulted);

            }
            else if (!MessageQueue.HasValue())
            {
                Task.Delay(DelayTimeWhenNoMessageCome).GetAwaiter().GetResult();
            }
            else
            {
                Task.Delay(TimeSpan.FromMilliseconds(10)).GetAwaiter().GetResult();
            }
        }
    }
}