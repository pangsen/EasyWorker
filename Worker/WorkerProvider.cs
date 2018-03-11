namespace Worker
{
    public static class WorkerProvider
    {
        private static Worker Worker { get; set; }
        public static Worker GetWorker()
        {
            if (Worker == null)
            {
                Worker = WorkerOption.New.CreateWorker();
            }
            return Worker;
        }
    }
}