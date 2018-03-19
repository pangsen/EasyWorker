using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Worker.Interface;

namespace Worker.Implementation
{
    public class MultipleThreadConsumer : IConsumer
    {
        private readonly ILogger _logger;
        public IMessageQueue MessageQueue { get; set; }
        public IHandlerManager HandlerManager { get; set; }
        public int PendingTaskCount { get; set; }
        private bool IsStop { get; set; }
        private TimeSpan DelayTimeWhenNoMessageCome { get; set; }
        private int MaxTaskCount { get; set; }
        private static readonly object Lock = new object();
        public MultipleThreadConsumer(IMessageQueue messageQueue, IHandlerManager handlerManager, ILogger logger, int maxTaskCount = 10, int delaySecondsWhenNoMessageCome = 5)
        {
            _logger = logger;
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

        public void SetMaxTaskCount(int maxTaskCount)
        {
            MaxTaskCount = maxTaskCount;
        }

        public void SetDelaySecondsWhenNoMessageCome(int delaySecondsWhenNoMessageCome)
        {
            DelayTimeWhenNoMessageCome = TimeSpan.FromSeconds(delaySecondsWhenNoMessageCome);
        }


        private void IncreasePendingTaskCount()
        {
            lock (Lock)
            {
                PendingTaskCount++;
            }
        }
        private void DecreasePendingTaskCount()
        {
            lock (Lock)
            {
                PendingTaskCount--;
            }
        }
        private void Run()
        {
            if (PendingTaskCount < MaxTaskCount && MessageQueue.HasValue())
            {
                var message = (dynamic)MessageQueue.Dequeue();
                IncreasePendingTaskCount();

                Task.Run(() =>
                    {

                        HandlerManager.Handle(message, CancellationToken.None);
                        DecreasePendingTaskCount();
                    })
                    .ContinueWith((t) =>
                    {
                        _logger.Write(t?.Exception?.InnerException?.Message);
                        DecreasePendingTaskCount();
                    }, TaskContinuationOptions.OnlyOnFaulted);
            }
            else if (PendingTaskCount == 0 && !MessageQueue.HasValue())
            {
                Task.Delay(DelayTimeWhenNoMessageCome).GetAwaiter().GetResult();
            }
            else
            {
                Task.Delay(TimeSpan.FromMilliseconds(1)).GetAwaiter().GetResult();
            }
        }
    }
}