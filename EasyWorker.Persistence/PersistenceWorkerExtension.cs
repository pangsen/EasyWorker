namespace Worker.Persistence
{
    public static class PersistenceWorkerExtension
    {
        public static WorkerOption UsePersistenceWorker(this WorkerOption options, string workerIdentity = "DefaultWorker")
        {

            options.MessageManagerFatory = (ops) => new PersistenceMessageManager(ops.Logger, workerIdentity);
            options.WorkerFactory = (ops) => new PersistenceWorker(ops);
            return options;
        }
    }
}