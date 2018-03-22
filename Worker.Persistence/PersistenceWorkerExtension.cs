namespace Worker.Persistence
{
    public static class PersistenceWorkerExtension
    {
        public static WorkerOption UsePersistenceWorker(this WorkerOption options)
        {

            options.MessageQueueFatory = (ops) => new PersistenceMessageQueue(ops.Logger);
            options.MessageManagerFatory = (ops) => new PersistenceMessageManager();
            options.WorkerFactory = (ops) => new PersistenceWorker(ops);
            return options;
        }
    }
}