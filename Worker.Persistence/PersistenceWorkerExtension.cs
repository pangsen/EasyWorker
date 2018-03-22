namespace Worker.Persistence
{
    public static class PersistenceWorkerExtension
    {
        public static WorkerOption UsePersistenceWorker(this WorkerOption options)
        {

            options.MessageQueueFatory = (ops) => new PersistenceMessageQueue(ops.Logger);
            options.HistoryMessageManagerFatory = (ops) => new PersistenceHistoryMessageManager();
            options.ErrorMessageManagerFatory = (ops) => new PersistenceErrorMessageManager();
            options.WorkerFactory = (ops) => new PersistenceWorker(ops);
            return options;
        }
    }
}