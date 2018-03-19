using System.Collections.Generic;
using System.Threading;
using Worker.Interface;

namespace Worker.Implementation
{
    public class DefaultHandlerManager : IHandlerManager
    {
        private readonly ILogger _logger;
        public List<object> Handlers { get; set; }

        public DefaultHandlerManager(ILogger logger)
        {
            _logger = logger;
            Handlers = new List<object>();
        }
        public void AddHandler<T>(IHander<T> handler) where T : IMessgae
        {
            Handlers.Add(handler);
        }

        public void Handle<T>(T message,CancellationToken cancellationToken) where T : IMessgae
        {
            Handlers.ForEach(a =>
            {
                if (typeof(IHander<>).MakeGenericType(message.GetType()).IsInstanceOfType(a))
                {
                    var handler = (IHander<T>)a;
                    handler.Handle(message, cancellationToken);
                }
            });
        }

       
    }
}
