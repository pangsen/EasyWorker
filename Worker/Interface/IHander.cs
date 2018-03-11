namespace Worker
{
    public interface IHander<in T> where T : IMessgae
    {
        void Handle(T message);

    }
}