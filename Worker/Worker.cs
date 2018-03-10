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
    public class Worker
    {
        private object _lock = new object();
        private Queue<IMessgae> _queue { get; set; }
        private List<IMessgae> _pending { get; set; }
        private List<object> _handlers { get; set; }
        private int _maxTaskCount { get; set; } = 10;
        private bool _stop { get; set; }
        private TimeSpan _delayTimeSpan { get; set; } = TimeSpan.FromSeconds(5);

        public Worker()
        {
            _queue = new Queue<IMessgae>();
            _pending = new List<IMessgae>();
            _handlers = new List<object>();
        }
        public void AddJob<T>(T t) where T : IMessgae
        {
            lock (_lock)
            {
                _queue.Enqueue(t);
            }

        }
        public void AddJobs<T>(IEnumerable<T> list) where T : IMessgae
        {
            lock (_lock)
            {
                foreach (var item in list)
                {
                    AddJob(item);
                }
            }
        }
        public void AddHandler<T>(IHander<T> handler) where T : IMessgae
        {
            _handlers.Add(handler);
        }
        public void Start()
        {

            while (!_stop)
            {
                if (_pending.Count < _maxTaskCount && _queue.Any())
                {
                    IMessgae message;
                    lock (_lock)
                    {
                        message = _queue.Dequeue();
                    }

                    _pending.Add(message);

                    Task.Run(() =>
                    {
                        Handle((dynamic)message);
                        _pending.Remove(message);
                    });
                }
                else if (!_queue.Any())
                {
                    Task.Delay(_delayTimeSpan).GetAwaiter().GetResult();
                }
                else
                {
                    Task.Delay(TimeSpan.FromMilliseconds(10)).GetAwaiter().GetResult();
                }
            }
            while (_pending.Any())
            {
                Task.Delay(TimeSpan.FromMilliseconds(100)).GetAwaiter().GetResult();
            }
        }
        public void Stop()
        {
            _stop = true;
        }
        public void SetDelayTime(TimeSpan delayTime)
        {
            _delayTimeSpan = delayTime;
        }
        public bool HasValue()
        {
            return _queue.Any();
        }
        private void Handle<T>(T message) where T : IMessgae
        {
            _handlers.ForEach(a =>
            {
                if (typeof(IHander<>).MakeGenericType(message.GetType()).IsAssignableFrom(a.GetType()))
                {
                    var handler = (IHander<T>)a;
                    handler.Handle(message);
                }
            });
        }
    }
}
