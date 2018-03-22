using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Worker.Implementation;
using Worker.Interface;

namespace Worker
{
    public interface IWorker
    {
        void AddHandler<T>(IHander<T> handler) where T : Messgae;
        void Publish<T>(T message) where T : Messgae;
        void Publish<T>(IEnumerable<T> messages) where T : Messgae;
        void Start();
        void Stop();
        void WaitUntilNoMessage();
       
    }
    public class Worker : IWorker, IDisposable
    {

        private readonly WorkerOption _options;
        protected IMessageQueue MessageQueue => _options.MessageQueue;
        protected IConsumer Consumer => _options.Consumer;
        protected IHandlerManager HandlerManager => _options.HandlerManager;
        protected IHistoryMessageManager HistoryMessageManager => _options.HistoryMessageManager;
        protected IErrorMessageManager ErrorMessageManager => _options.ErrorMessageManager;
        public Worker(WorkerOption options)
        {
            _options = options;
        }

        public void AddHandler<T>(IHander<T> handler) where T : Messgae
        {
            HandlerManager.AddHandler(handler);
        }

        public void Publish<T>(T message) where T : Messgae
        {
            MessageQueue.Enqueue(message);
        }

        public void Publish<T>(IEnumerable<T> items) where T : Messgae
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
                Task.Delay(TimeSpan.FromMilliseconds(1)).GetAwaiter().GetResult();
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
