using System.Threading;
using Worker.Implementation;

namespace Worker.Interface
{
    public interface IHander<in T> where T : Message
    {
        void Handle(T message,CancellationToken cancellationToken);

    }
}