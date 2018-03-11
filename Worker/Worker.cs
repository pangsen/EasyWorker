using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker
{

    public interface IMessgae { }
    public interface IHander<in T> where T : IMessgae
    {
        void Handle(T message);
    }
    public class Worker : IDisposable
    {
        private readonly object _lock = new object();
        private Queue<IMessgae> Queue { get; set; }
        private List<Task> PendingTasks { get; set; }
        private List<object> Handlers { get; set; }

        private bool IsStop { get; set; }
        private int MaxTaskCount { get; set; }
        private TimeSpan DelayTimeWhenNoMessageCome { get; set; }

        public Worker(int maxThreadCount = 10, int delaySecondsWhenNoMessageCome = 5)
        {
            MaxTaskCount = maxThreadCount;
            DelayTimeWhenNoMessageCome = TimeSpan.FromSeconds(delaySecondsWhenNoMessageCome);
            Queue = new Queue<IMessgae>();
            PendingTasks = new List<Task>();
            Handlers = new List<object>();
        }
        public void Publish<T>(T t) where T : IMessgae
        {
            if (IsStop) { throw new Exception("Worker already stop"); }
            lock (_lock)
            {
                Queue.Enqueue(t);
            }

        }
        public void Publish<T>(IEnumerable<T> list) where T : IMessgae
        {
            lock (_lock)
            {
                foreach (var message in list)
                {
                    Publish(message);
                }
            }
        }
        public void AddHandler<T>(IHander<T> handler) where T : IMessgae
        {
            Handlers.Add(handler);
        }


        public void WorkUntilComplete()
        {
            while (HasValue())
            {
                HandleMessageFromQueue();
            }
            Task.WaitAll(PendingTasks.ToArray());
        }
        public void Start()
        {
            Task.Run(() =>
            {
                while (!IsStop)
                {
                    HandleMessageFromQueue();
                }
            });
        }
        public void Stop()
        {
            IsStop = true;
        }
        public bool HasValue()
        {
            lock (_lock)
            {
                return Queue.Any();
            }
        }

        public void Dispose()
        {
            Stop();
        }

        private void Handle<T>(T message) where T : IMessgae
        {
            Handlers.ForEach(a =>
            {
                if (typeof(IHander<>).MakeGenericType(message.GetType()).IsInstanceOfType(a))
                {
                    var handler = (IHander<T>)a;
                    handler.Handle(message);
                }
            });
        }

        private void HandleMessageFromQueue()
        {
            bool isThereQueuedMessage;
            int pendingTasksCount;

            lock (_lock)
            {
                isThereQueuedMessage = Queue.Any();
                pendingTasksCount = PendingTasks.Count;
                if (isThereQueuedMessage && pendingTasksCount < MaxTaskCount)
                {
                    var message = Queue.Dequeue();
                    PendingTasks.Add(
                        Task.Run(() =>
                        {
                            Handle((dynamic)message);
                        })
                    );

                }
                PendingTasks = PendingTasks.Where(a => !a.IsCompleted).ToList();
            }
            if (!isThereQueuedMessage)
            {
                Task.Delay(DelayTimeWhenNoMessageCome).GetAwaiter().GetResult();
            }
            if (pendingTasksCount == MaxTaskCount)
            {
                 Task.Delay(TimeSpan.FromMilliseconds(10)).GetAwaiter().GetResult();
            }
           
            
        }

        ~Worker()
        {
            Dispose();
        }
    }
}
