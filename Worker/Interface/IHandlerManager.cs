using System.Threading;
using Worker.Implementation;

namespace Worker.Interface
{
    public interface IHandlerManager
    {

        void AddHandler<T>(IHander<T> handler) where T : Messgae;

        void Handle<T>(T message, CancellationToken cancellationToken) where T : Messgae;
    }
}