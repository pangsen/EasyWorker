using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Worker.Implementation;
using Worker.Interface;

namespace Worker
{
    public interface IWorker
    {
        void AddHandler<T>(IHander<T> handler) where T : Message;
        void Publish<T>(T message) where T : Message;
        void Publish<T>(IEnumerable<T> messages) where T : Message;
        void Start();
        void Stop();
        void WaitUntilNoMessage();
        List<Message> GetErrorMessages();
        List<Message> GetHistoryMessages();
        List<Message> GetPendingMessages();
        List<Message> GetQueuedMessages();
        void RePublishErrorMessages();
    }
    public class Worker : IWorker, IDisposable
    {

        private readonly WorkerOption _options;
        protected IQueueMessager  QueueMessager => _options.MessageManager.QueueMessager;
        protected IConsumer Consumer => _options.Consumer;
        protected IHandlerManager HandlerManager => _options.HandlerManager;
        protected IMessageManager MessageManager => _options.MessageManager;
        public Worker(WorkerOption options)
        {
            _options = options;
           
        }
        public void AddHandler<T>(IHander<T> handler) where T : Message
        {
            HandlerManager.AddHandler(handler);
        }
        public void Publish<T>(T message) where T : Message
        {
            QueueMessager.Enqueue(message);
        }
        public void Publish<T>(IEnumerable<T> items) where T : Message
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
            while (QueueMessager.HasValue() || Consumer.PendingTaskCount > 0)
            {
                Task.Delay(TimeSpan.FromMilliseconds(1)).GetAwaiter().GetResult();
            }
        }
        public List<Message> GetErrorMessages()
        {
            return MessageManager.GetErrorMessages();
        }
        public List<Message> GetHistoryMessages()
        {
            return MessageManager.GetHistoryMessages();
        }
        public List<Message> GetPendingMessages()
        {
            return MessageManager.GetPendingMessages();
        }
        public List<Message> GetQueuedMessages()
        {
            return QueueMessager.GetQueuedMessages();
        }

        public void RePublishErrorMessages()
        {
            var errorQueue = new Queue<Message>(MessageManager.GetErrorMessages());
            while (errorQueue.Count > 0)
            {
                var message = errorQueue.Dequeue();
                MessageManager.RemoveErrorMessage(message.Id);
                Publish(message);
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
