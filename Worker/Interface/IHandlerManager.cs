namespace Worker
{
    public interface IHandlerManager
    {

        void AddHandler<T>(IHander<T> handler) where T : IMessgae;

        void Handle<T>(T message) where T : IMessgae;
    }
}