using System.Threading;
using Worker.Implementation;

namespace Worker.Interface
{
    public interface IHander<in T> where T : Messgae
    {
        void Handle(T message,CancellationToken cancellationToken);

    }
}