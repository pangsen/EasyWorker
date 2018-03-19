using System.Threading;

namespace Worker.Interface
{
    public interface IHandlerManager
    {

        void AddHandler<T>(IHander<T> handler) where T : IMessgae;

        void Handle<T>(T message, CancellationToken cancellationToken) where T : IMessgae;
    }
}