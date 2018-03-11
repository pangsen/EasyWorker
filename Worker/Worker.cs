using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Worker
{
    public class Worker : IDisposable
    {
        private readonly WorkerOption _options;
        private IMessageQueue MessageQueue => _options.MessageQueue;
        private IConsumer Consumer => _options.Consumer;
        private IHandlerManager HandlerManager => _options.HandlerManager;

        public Worker(WorkerOption options)
        {
            _options = options;
        }

        public void AddHandler<T>(IHander<T> handler) where T : IMessgae
        {
            HandlerManager.AddHandler(handler);
        }

        public void Publish<T>(T message) where T : IMessgae
        {
            MessageQueue.Enqueue(message);
        }

        public void Publish<T>(IEnumerable<T> items) where T : IMessgae
        {
            foreach (var item in items)
            {
                Publish(item);
            }
        }

        public void Start()
        {
            Consumer.Start();
        }
        public void Stop()
        {
            Consumer.Stop();
        }

        public void WaitUntilNoMessage()
        {
            while (MessageQueue.HasValue() || Consumer.PendingTaskCount > 0)
            {
                Task.Delay(TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();
            }
        }
        public void Dispose()
        {
            Stop();
        }


        ~Worker()
        {
            Dispose();
        }
    }
}
