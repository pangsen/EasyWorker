using System.Threading;

namespace Worker.Interface
{
    public interface IHander<in T> where T : IMessgae
    {
        void Handle(T message,CancellationToken cancellationToken);

    }
}