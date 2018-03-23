namespace Worker
{
    public static class WorkerProvider
    {
        private static IWorker Worker { get; set; }
        public static IWorker GetWorker()
        {
            if (Worker == null)
            {
                Worker = WorkerOption.New.CreateWorker<IWorker>();
            }
            return Worker;
        }
    }
}