using System.Collections.Generic;

namespace Worker
{
    public class DefaultHandlerManager : IHandlerManager
    {
        public List<object> Handlers { get; set; }

        public DefaultHandlerManager()
        {
            Handlers = new List<object>();
        }
        public void AddHandler<T>(IHander<T> handler) where T : IMessgae
        {
            Handlers.Add(handler);
        }

        public void Handle<T>(T message) where T : IMessgae
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
    }
}